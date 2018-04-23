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
    public class EngagementsController : BaseController
    {
        /// <summary>
        /// Get all Engagements.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 


        public IHttpActionResult Get(int page = 0, int pageSize = 10, int sort = 0, string searchBy = "", string filter = "")
        {
            IFunctionalityHelpers<Engagement, EngagementModel> Sorting = new EngagementFunctionalityHelper();
            var query = TimeUnit.Engagements.Get();

            query = Sorting.Filter(query, searchBy, filter);
            query = Sorting.Sort(query, sort);
            var list = Sorting.Paginate(query, pageSize, page);

            Header header = new Header(pageSize, Sorting.TotalPages, page, sort, query.Count());
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("ENGAGEMENT CONTROLLER: Get() Called.", "INFO");
            return Ok(list);
        }

        /// <summary>
        /// Get Engagement by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 


        public IHttpActionResult GetById(int id)
        {
            Engagement engagement = TimeUnit.Engagements.Get(id);
            if (engagement == null)
            {   Utility.Log($"ENGAGEMENT CONTROLLER: Get() Cannot be called on this ID: {id}.", "WARN");
                return NotFound();
            }
                
            else
            {
                Utility.Log($"ENGAGEMENT CONTROLLER: Get() Called on ID:  {id}.", "INFO");
                return Ok(TimeFactory.Create(engagement));
            }

        }

        /// <summary>
        /// Insert Engagement.
        /// </summary>
        /// <param name="engagement"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Post([FromBody] Engagement engagement)
        {
            try
            {
                List<string> errors = ValidateExtensions.ValidateEngagements(engagement);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                engagement.Team = TimeUnit.Teams.Get(engagement.Team.Id);
                engagement.Role = TimeUnit.Roles.Get(engagement.Role.Id);
                engagement.Employee = TimeUnit.Employees.Get(engagement.Employee.Id);

                TimeUnit.Engagements.Insert(engagement);
                TimeUnit.Save();
                Utility.Log($"ENGAGEMENT CONTROLLER: Post Called on Engagement, Successfully added: {engagement.Id}.", "INFO");
                return Ok(TimeFactory.Create(engagement));
            }

            catch (Exception ex)

            {
                Utility.Log($"ENGAGEMENT CONTROLLER: Post Cannot be called on Engagement.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Engagement by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="engagement"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Put([FromBody] Engagement engagement, int id)
        {
            try
            {

                List<string> errors = ValidateExtensions.ValidateEngagements(engagement);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                engagement.Team = TimeUnit.Teams.Get(engagement.Team.Id);
                engagement.Role = TimeUnit.Roles.Get(engagement.Role.Id);
                engagement.Employee = TimeUnit.Employees.Get(engagement.Employee.Id);

                if (TimeUnit.Engagements.Get(id) == null) return NotFound();

                TimeUnit.Engagements.Update(engagement, id);
                TimeUnit.Save();
                Utility.Log($"ENGAGEMENT CONTROLLER: Put Called on Engagement, Successfully updated id: {id}.", "INFO");
                return Ok(TimeFactory.Create(engagement));
            }
            catch (Exception ex)
            {
                Utility.Log($"ENGAGEMENT CONTROLLER: Put Cannot be called on Engagement.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Engagement by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Delete(int id)
        {

            try
            {
                Engagement engagement = TimeUnit.Engagements.Get(id);
                //engagement.Employee = TimeUnit.Employees.Get(engagement.Employee.Id);
                //engagement.Team = TimeUnit.Teams.Get(engagement.Team.Id);
                //engagement.Role = TimeUnit.Roles.Get(engagement.Role.Id);
                if (engagement == null) return NotFound();
                TimeUnit.Engagements.Delete(engagement);
                TimeUnit.Save();
                Utility.Log($"ENGAGEMENT CONTROLLER: Deleted Called on Engagement, Successfully deleted id: {id}.", "INFO");
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
