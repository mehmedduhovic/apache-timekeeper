using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.API.Controllers.Helpers.Validations;
using TimeKeeper.API.Helpers;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers
{

   // [TimeAuth("Administrator")]

    public class EmployeesController : BaseController
    {
        /// <summary>
        /// Get all Employees.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 
        public IHttpActionResult Get(int page = 0, int pageSize = 10, int sort = 0, string searchBy="", string filter= "")
        {
            IFunctionalityHelpers<Employee, EmployeeModel> employeeHelper = new EmployeeFunctionalityHelper();
            
            var query = TimeUnit.Employees.Get();

            query = employeeHelper.Filter(query, searchBy, filter);
            query = employeeHelper.Sort(query, sort);
            var list = employeeHelper.Paginate(query, pageSize, page);

         
            //if (filter != "") query = query.Where(e => e.LastName.Contains(filter));

            //var list = Sorting.SortAndPaginate(query, sort, page, pageSize, filter);
           
           // switch (sort)
           // {
           //     case 1: query = query.OrderBy(x => x.LastName); break;
           //     case 2: query = query.OrderBy(x => x.BirthDate); break;
           //     default: query = query.OrderBy(x => x.Id); break;
           // }  

           //var header = new
           // {
           //     nextPage = (page >=  totalPages - 1) ? -1 : page + 1,
           //     previousPage = page - 1,
           //     pageSize = pageSize,
           //     totalPages = totalPages,
           //     page = page,
           //     sort = sort
           // };
            

            Header header = new Header(pageSize, employeeHelper.TotalPages, page, sort, query.Count());
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("EMPOLOYEE CONTROLLER: Get() Called.", "INFO");
            return Ok(list);
        }
        [Route("api/employees/all")]
        public IHttpActionResult GetAll(string all = "")
        {
            //var list = TimeUnit.Employees.Get().ToList();
            var list = TimeUnit.Employees.Get().ToList().Select(e => TimeFactory.Create(e)).ToList();
            return Ok(list);
        }

        /// <summary>
        /// Get Employee by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 
        //[ScopeAuthorize("read")]
        public IHttpActionResult GetById(int id)
        {
            //var claimsPrincipal = User as ClaimsPrincipal;
            //string username = claimsPrincipal.FindFirst
            //("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            //Employee employee = TimeUnit.Employees.Get(x => x.Email == username).FirstOrDefault();
            Employee employee = TimeUnit.Employees.Get(id);
            if (employee == null)
            {
                Utility.Log($"EMPOLOYEE CONTROLLER: Get() Cannot be called on this ID: {id}.", "WARN");
                return NotFound();
            }
            else
            {
                employee.Image = employee.ConvertToBase64();
                Utility.Log($"EMPOLOYEE CONTROLLER: Get() Called on ID:  {id}.", "WARN");
                return Ok(TimeFactory.Create(employee, true));
            }
        }

        /// <summary>
        /// Insert Employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 
        /// 

        [HttpPost]
        public IHttpActionResult Post([FromBody] Employee employee)
        {
            try
            {
                
                List<string> errors = ValidateExtensions.ValidateEmployee(employee);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }
                

                employee.Roles = TimeUnit.Roles.Get(employee.Roles.Id);
                employee.Image = employee.ConvertAndSave();
                TimeUnit.Employees.Insert(employee);
                TimeUnit.Save();
                Utility.Log($"EMPOLOYEE CONTROLLER: Post Called on Employee, Successfully added: {employee.FirstName + " " + employee.LastName}.", "INFO");
                return Ok(TimeFactory.Create(employee));
            }
            catch (Exception ex)
            {
                Utility.Log($"EMPOLOYEE CONTROLLER: Post Cannot be called on Employee.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Employee by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Put([FromBody] Employee employee, int id)
        {
            try
            {
                Employee oldEmployee = TimeUnit.Employees.Get(id);
                if (oldEmployee == null) return NotFound();
                employee = FillEmployeeWithOldData(employee, oldEmployee);

                employee.Roles = TimeUnit.Roles.Get(employee.Roles.Id);
                employee.Image = employee.ConvertAndSave();

                List<string> errors = ValidateExtensions.ValidateEmployee(employee);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                if (TimeUnit.Employees.Get(id) == null) return NotFound();
                TimeUnit.Employees.Update(employee, id);
                TimeUnit.Save();
                Utility.Log($"EMPOLOYEE CONTROLLER: Put Called on Employee, Successfully updated id: {id}.", "INFO");
                return Ok(TimeFactory.Create(employee));
            }
            catch (Exception ex)
            {
                Utility.Log($"EMPOLOYEE CONTROLLER: Put Cannot be called on Employee.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Employee by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Delete(int id)
        {
            try
            {
                Employee employee = TimeUnit.Employees.Get(id);
                if (employee == null) return NotFound();
                //List<Day> days = employee.Days.ToList();
                //List<Engagement> engagements = employee.Engagements.ToList();

                TimeUnit.Employees.Delete(employee);
                TimeUnit.Save();
                Utility.Log($"EMPOLOYEE CONTROLLER: Deleted Called on Employee, Successfully deleted id: {id}.", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Employee FillEmployeeWithOldData(Employee newEmployee, Employee oldEmployee)
        {
            newEmployee.Id = oldEmployee.Id;
            newEmployee.CreatedBy = oldEmployee.CreatedBy;
            newEmployee.CreatedOn = oldEmployee.CreatedOn;


            if (newEmployee.Days.Count == 0 && oldEmployee.Days.Count != 0)
                newEmployee.Days = oldEmployee.Days;

            if (newEmployee.Engagements.Count == 0 && oldEmployee.Engagements.Count != 0)
                newEmployee.Engagements = oldEmployee.Engagements;

            if (newEmployee.Roles == null && oldEmployee.Roles != null)
                newEmployee.Roles = TimeUnit.Roles.Get(oldEmployee.Roles.Id);

            if (newEmployee.Image == null && oldEmployee.Image != null)
                newEmployee.Image = oldEmployee.Image;

            if (newEmployee.FirstName == null && oldEmployee.FirstName != null)
                newEmployee.FirstName = oldEmployee.FirstName;

            if (newEmployee.LastName == null && oldEmployee.LastName != null)
                newEmployee.LastName = oldEmployee.LastName;

            if (newEmployee.Phone == null && oldEmployee.Phone != null)
                newEmployee.Phone = oldEmployee.Phone;

            if (newEmployee.Salary == null && oldEmployee.Salary != null)
                newEmployee.Salary = oldEmployee.Salary;

            if (newEmployee.Status == null && oldEmployee.Status != null)
                newEmployee.Status = oldEmployee.Status;

            if (newEmployee.Email == null && oldEmployee.Email != null)
                newEmployee.Email = oldEmployee.Email;

            if (newEmployee.BeginDate == null && oldEmployee.BeginDate != null)
                newEmployee.BeginDate = oldEmployee.BeginDate;

            if (newEmployee.BirthDate == null && oldEmployee.BirthDate != null)
                newEmployee.BirthDate = oldEmployee.BirthDate;

            if (newEmployee.EndDate == null && oldEmployee.EndDate != null)
                newEmployee.EndDate = oldEmployee.EndDate;

            if (newEmployee.Password == null && oldEmployee.Password != null)
                newEmployee.Password = oldEmployee.Password;

            return newEmployee;
        }
    }
}
