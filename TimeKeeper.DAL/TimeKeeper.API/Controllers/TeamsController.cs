using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.API.Controllers.Helpers.Validations;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.API.Controllers
{
    public class TeamsController : BaseController
    {
        /// <summary>
        /// Get all Teams.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>

        public IHttpActionResult Get(int page = 0, int pageSize = 10, int sort = 0, string searchBy = "", string filter = "")
        {

            IFunctionalityHelpers<Team, TeamModel> teamsHelper = new TeamFuctionalityHelper();

            var query = TimeUnit.Teams.Get();

            query = teamsHelper.Filter(query, searchBy, filter);
            query = teamsHelper.Sort(query, sort);
            var list = teamsHelper.Paginate(query, pageSize, page);


            Header header = new Header(pageSize, teamsHelper.TotalPages, page, sort, query.Count());
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            //var list = TimeUnit.Teams.Get().ToList().Select(t => TimeFactory.Create(t)).ToList();
            Utility.Log("TEAM CONTROLLER: Get() Called.", "INFO");
            return Ok(list);
        }


        [Route("api/teams/all")]
        public IHttpActionResult GetAll(string all = "")
        {
            var list = TimeUnit.Teams.Get().ToList().Select(e => TimeFactory.Create(e)).ToList();
            return Ok(list);
        }

        /// <summary>
        /// Get Team by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>

        public IHttpActionResult GetById(string id)
        {
            Team team = TimeUnit.Teams.Get(id);
            if (team == null)
            {
                Utility.Log($"TEAM CONTROLLER: Get() Cannot be called on this ID: {id}.", "WARN");
                return NotFound();
            }
            else
            {
                Utility.Log($"TEAM CONTROLLER: Get() Called on ID:  {id}.", "INFO");
                return Ok(TimeFactory.Create(team));
            }
               

        }

        /// <summary>
        /// Insert Team.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>

        public IHttpActionResult Post([FromBody] Team team)
        {
            try
            {
                List<string> errors = ValidateExtensions.ValidateTeam(team);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                TimeUnit.Teams.Insert(team);
                TimeUnit.Save();
                Utility.Log($"TEAM CONTROLLER: Post Called on Team, Successfully added: {team.Name}.", "INFO");
                return Ok(TimeFactory.Create(team));
            }

            catch (Exception ex)

            {
                Utility.Log($"TEAM CONTROLLER: Post Cannot be called on Team.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Team by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>

        public IHttpActionResult Put([FromBody] Team team, string id)
        {
            List<string> errors = ValidateExtensions.ValidateTeam(team);

            if (errors.Count > 0)
            {
                return Content(HttpStatusCode.BadRequest, errors);
            }


            try
            {

                if (TimeUnit.Teams.Get(id) == null) return NotFound();
                TimeUnit.Teams.Update(team, id);
                TimeUnit.Save();
                Utility.Log($"ROLE CONTROLLER: Put Called on Role, Successfully updated id: {id}.", "INFO");
                return Ok(TimeFactory.Create(team));
            }
            catch (Exception ex)
            {
                Utility.Log($"ROLE CONTROLLER: Put Cannot be called on Role.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Team by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>

        public IHttpActionResult Delete(string id)
        {

            try
            {
                Team team = TimeUnit.Teams.Get(id);
                if (team == null) return NotFound();
                TimeUnit.Teams.Delete(team);
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