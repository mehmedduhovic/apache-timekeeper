using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.API.Controllers.Helpers.Validations;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers
{
    public class TasksController : BaseController
    {
        /// <summary>
        /// Get all Tasks.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Get(int page = 0, int pageSize = 10, int sort = 0, string searchBy = "", string filter = "")
        {
           
            IFunctionalityHelpers<Task, TaskModel> tasksHelper = new TasksFunctionalityHelper();

            var query = TimeUnit.Tasks.Get();

            query = tasksHelper.Filter(query, searchBy, filter);
            query = tasksHelper.Sort(query, sort);
            var list = tasksHelper.Paginate(query, pageSize, page);

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


            Header header = new Header(pageSize, tasksHelper.TotalPages, page, sort, query.Count());
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("TASK CONTROLLER: Get() Called.", "INFO");
            return Ok(list);

        }

        /// <summary>
        /// Get Task by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult GetById(int id)
        {
 

            Task task = TimeUnit.Tasks.Get(id);
            if (task == null)
            {
                Utility.Log($"ROLE CONTROLLER: Get() Cannot be called on this ID: {id}.", "WARN");
                return NotFound();
            }
            else
            {
                Utility.Log($"ROLE CONTROLLER: Get() Called on ID:  {id}.", "INFO");
                return Ok(TimeFactory.Create(task));
            }
        }

        /// <summary>
        /// Insert Task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Post([FromBody] Task task)
        {
            try
            {
                List<string> errors = ValidateExtensions.ValidateTask(task);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                task.Day = TimeUnit.Days.Get(task.Day.Id);
                task.Project = TimeUnit.Projects.Get(task.Day.Id);

                TimeUnit.Tasks.Insert(task);
                TimeUnit.Save();
                Utility.Log($"ROLE CONTROLLER: Post Called on Role, Successfully added: {task.Description}.", "INFO");
                return Ok(TimeFactory.Create(task));
            }
            catch (Exception ex)
            {
                Utility.Log($"ROLE CONTROLLER: Post Cannot be called on Role.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Task by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Put([FromBody] Task task, int id)
        {
            try
            {
                List<string> errors = ValidateExtensions.ValidateTask(task);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                task.Day = TimeUnit.Days.Get(task.Day.Id);
                task.Project = TimeUnit.Projects.Get(task.Project.Id);

                if (TimeUnit.Tasks.Get(id) == null) return NotFound();
                TimeUnit.Tasks.Update(task, id);
                TimeUnit.Save();
                Utility.Log($"ROLE CONTROLLER: Put Called on Role, Successfully updated id: {id}.", "INFO");
                return Ok(TimeFactory.Create(task));
            }
            catch (Exception ex)
            {
                Utility.Log($"ROLE CONTROLLER: Put Cannot be called on Role.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Task by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Delete(int id)
        {
            try
            {
                Task task = TimeUnit.Tasks.Get(id);
                if (task == null) return NotFound();
                TimeUnit.Tasks.Delete(task);
                TimeUnit.Save();
                Utility.Log($"ROLE CONTROLLER: Deleted Called on Role, Successfully deleted id: {id}.", "INFO");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
