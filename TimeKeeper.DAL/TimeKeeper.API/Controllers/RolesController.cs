using System;
using System.Linq;
using System.Web.Http;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;
using System.Collections.Generic;
using TimeKeeper.API.Controllers.Helpers;
using System.Web;
using Newtonsoft.Json;
using TimeKeeper.API.Controllers.Helpers.Validations;
using System.Net;

namespace TimeKeeper.API.Controllers
{
    //[RoutePrefix("api/config")]
    public class RolesController : BaseController
    {
        /// <summary>
        /// Get all Roles.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Get(int page = 0, int pageSize = 14, int sort = 0, string searchBy = "", string filter = "")
        {
            IFunctionalityHelpers<Role, RoleModel> Sorting = new RoleFunctionalityHelper();
            var query = TimeUnit.Roles.Get();

            query = Sorting.Filter(query, searchBy, filter);
            query = Sorting.Sort(query, sort);
            var list = Sorting.Paginate(query, pageSize, page);

            Header header = new Header(pageSize, Sorting.TotalPages, page, sort, query.Count());
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("ROLE CONTROLLER: Get() Called.", "INFO");
            return Ok(list);
        }

        //Route("{id}")
        //get
        //list<rolemodel> list = timeunit.roles.get(x => x.Type == (RoleType)type).Select()
        //return Ok(list)

        //Route[id]
        //get

        /// <summary>
        /// Get Role by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 


        public IHttpActionResult GetById(string id)
        {
            Role role = TimeUnit.Roles.Get(id);
            if (role == null) {
                Utility.Log($"ROLE CONTROLLER: Get() Cannot be called on this ID: {id}.", "WARN");
                return NotFound();
                }
            else {
                Utility.Log($"ROLE CONTROLLER: Get() Called on ID:  {id}.", "WARN");
                return Ok(TimeFactory.Create(role));
            }
                
        }

        /// <summary>
        /// Insert Role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Post([FromBody] Role role)
        {
            try
            {
                List<string> errors = ValidateExtensions.ValidateRole(role);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                TimeUnit.Roles.Insert(role);
                TimeUnit.Save();
                Utility.Log($"ROLE CONTROLLER: Post Called on Role, Successfully added: {role.Name}.", "INFO");
                return Ok(TimeFactory.Create(role));
            }
            catch (Exception ex)
            {
                Utility.Log($"ROLE CONTROLLER: Post Cannot be called on Role.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Role by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Put([FromBody] Role role, string id)
        {
            try
            {
                List<string> errors = ValidateExtensions.ValidateRole(role);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }
                if (TimeUnit.Roles.Get(id) == null) return NotFound();
                TimeUnit.Roles.Update(role, id);
                TimeUnit.Save();
                Utility.Log($"ROLE CONTROLLER: Put Called on Role, Successfully updated id: {id}.", "INFO");
                return Ok(TimeFactory.Create(role));
            }
            catch (Exception ex)
            {
                Utility.Log($"ROLE CONTROLLER: Put Cannot be called on Role.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Role by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Delete(string id)
        {
            try
            {
                Role role = TimeUnit.Roles.Get(id);
                if (role == null) return NotFound();
                TimeUnit.Roles.Delete(role);
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

