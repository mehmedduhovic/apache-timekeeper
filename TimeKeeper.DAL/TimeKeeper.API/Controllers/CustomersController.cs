using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.API.Controllers.Helpers.Validations;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers
{
    public class CustomersController : BaseController
    {

        /// <summary>
        /// Get all Customers.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>         
        public IHttpActionResult Get(int page = 0, int pageSize = 20, int sort = 0, string searchBy = "", string filter = "")
        {
            IFunctionalityHelpers<Customer, CustomerModel> Sorting = new CustomerFunctionalityHelper();
            var query = TimeUnit.Customers.Get();

            query = Sorting.Filter(query, searchBy, filter);
            query = Sorting.Sort(query, sort);
            var list = Sorting.Paginate(query, pageSize, page);

            Header header = new Header(pageSize, Sorting.TotalPages, page, sort, query.Count());
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));
            
            Utility.Log("CUSTOMER CONTROLLER: Get() Called.", "INFO");
            return Ok(list);
        }

        /// <summary>
        /// Get Customers by Id.
        /// </summary>
        /// <param name = "id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult GetById(int id)
        {

            Customer customer = TimeUnit.Customers.Get(id);
            if (customer == null)
            {
                Utility.Log($"CUSTOMER CONTROLLER: Get() Cannot be called on this ID: {id}.", "WARN");
                return NotFound();
            }
            else
            {
                Utility.Log($"CUSTOMER CONTROLLER: Get() Called on ID:  {id}.", "WARN");
                return Ok(TimeFactory.Create(customer));
            }

        }

        /// <summary>
        /// Insert Customer.
        /// </summary>
        /// <param name = "customer"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Post([FromBody] Customer customer)
        {
            try
            {

                List<string> errors = ValidateExtensions.ValidateCustomer(customer);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                TimeUnit.Customers.Insert(customer);
                customer.Image = customer.ConvertAndSave();
                TimeUnit.Save();
                Utility.Log($"CUSTOMER CONTROLLER: Post Called on Customer, Successfully added: {customer.Name}.", "INFO");
                return Ok(TimeFactory.Create(customer));
            }
            catch (Exception ex)
            {
                Utility.Log($"CUSTOMER CONTROLLER: Post Cannot be called on Customer.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Customer by Id.
        /// </summary>
        /// <param name = "id"></param>
        /// <param name = "customer"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Put([FromBody] Customer customer, int id)
        {
            try
            {
                if (TimeUnit.Customers.Get(id) == null) return NotFound();

                List<string> errors = ValidateExtensions.ValidateCustomer(customer);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                TimeUnit.Customers.Update(customer, id);
                //customer.Image = customer.ConvertAndSave();
                TimeUnit.Save();
                Utility.Log($"CUSTOMER CONTROLLER: Put Called on Customer, Successfully updated id: {id}.", "INFO");
                return Ok(TimeFactory.Create(customer));
            }
            catch (Exception ex)
            {
                Utility.Log($"CUSTOMER CONTROLLER: Put Cannot be called on Customer.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Customer by Id.
        /// </summary>
        /// <param name = "id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Delete(int id)
        {
            try
            {
                Customer customer = TimeUnit.Customers.Get(id);
                if (customer == null) return NotFound();
                TimeUnit.Customers.Delete(customer);
                TimeUnit.Save();
                Utility.Log($"CUSTOMER CONTROLLER: Deleted Called on Customer, Successfully deleted id: {id}.", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
