using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.API.Controllers.Helpers.Validations;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers
{
    public class ProjectsController : BaseController
    {
        /// <summary>
        /// Get all Projects.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Get(int page = 0, int pageSize = 10, int sort = 0, string searchBy = "", string filter = "")
        {
            IFunctionalityHelpers<Project, ProjectModel> Sorting = new ProjectFunctionalityHelper();
            var query = TimeUnit.Projects.Get();

            query = Sorting.Filter(query, searchBy, filter);
            query = Sorting.Sort(query, sort);
            var list = Sorting.Paginate(query, pageSize, page);

            Header header = new Header(pageSize, Sorting.TotalPages, page, sort, query.Count());
            HttpContext.Current.Response.AddHeader("Pagination", JsonConvert.SerializeObject(header));

            Utility.Log("PROJECT CONTROLLER: Get() Called.", "INFO");
            return Ok(list);
        }

        /// <summary>
        /// Get Project by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult GetById(int id)
        {
            Project projects = TimeUnit.Projects.Get(id);
            if (projects == null) {
                Utility.Log($"PROJECTS CONTROLLER: Get() Cannot be called on this ID: {id}.", "WARN");
                return NotFound();
            }
            else { 
                Utility.Log($"PROJECTS CONTROLLER: Get() Called on ID:  {id}.", "INFO");
                return Ok(TimeFactory.Create(projects));
            }

        }

        [Route("api/projects/all")]
        public IHttpActionResult GetAll(string all = "")
        {
            var list = TimeUnit.Projects.Get().ToList().Select(e => TimeFactory.Create(e)).ToList();
            return Ok(list);
        }


        /// <summary>
        /// Insert Project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Post([FromBody] Project project)
        {
            try
            {
                List<string> errors = ValidateExtensions.ValidateProject(project);

                if (errors.Count > 0)
                {
                    return Content(HttpStatusCode.BadRequest, errors);
                }

                project.Customer = TimeUnit.Customers.Get(project.CustomerId);
                project.Team = TimeUnit.Teams.Get(project.TeamId);
                TimeUnit.Projects.Insert(project);
                TimeUnit.Save();
                Utility.Log($"PROJECT CONTROLLER: Post Called on Project, Successfully added: {project.Name}.", "INFO");
                return Ok(TimeFactory.Create(project));
            }

            catch (Exception ex)

            {
                Utility.Log($"PROJECT CONTROLLER: Post Cannot be called on Project.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update Project by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Put([FromBody] Project project, int id)
        {

            try
            {
                Project oldProject = TimeUnit.Projects.Get(id);
                if (oldProject == null) return NotFound();
                project = FillNewProjectWithOldData(project, oldProject);


                //project.Customer = TimeUnit.Customers.Get(project.Customer.Id);
                //project.Team = TimeUnit.Teams.Get(project.Team.Id);
                //project.CustomerId = project.Customer.Id;
                //project.TeamId = project.Team.Id;

                TimeUnit.Projects.Update(project, id);
                TimeUnit.Save();
                Utility.Log($"PROJECT CONTROLLER: Put Called on Project, Successfully updated id: {id}.", "INFO");
                return Ok(TimeFactory.Create(project));
            }
            catch (Exception ex)
            {
                Utility.Log($"PROJECT CONTROLLER: Put Cannot be called on Project.", "ERROR", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete Project by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 

        public IHttpActionResult Delete(int id)
        {

            try
            {
                Project projects = TimeUnit.Projects.Get(id);
                if (projects == null) return NotFound();
                TimeUnit.Projects.Delete(projects);
                TimeUnit.Save();
                Utility.Log($"PROJECT CONTROLLER: Deleted Called on Project, Successfully deleted id: {id}.", "INFO");
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Project FillNewProjectWithOldData(Project newProject, Project oldProject)
        {
            newProject.Id = oldProject.Id;
            newProject.CreatedBy = oldProject.CreatedBy;
            newProject.CreatedOn = oldProject.CreatedOn;

            if (newProject.Monogram == null && oldProject.Monogram != null)
                newProject.Monogram = oldProject.Monogram;

            if (newProject.Name == null && oldProject.Name != null)
                newProject.Name = oldProject.Name;

            if (newProject.StartDate == null && oldProject.StartDate != null)
                newProject.StartDate = oldProject.StartDate;

            if (newProject.Customer == null && oldProject.Customer != null)
                newProject.Customer = TimeUnit.Customers.Get(oldProject.Customer.Id);

            if (newProject.Description == null && oldProject.Description != null)
                newProject.Description = oldProject.Description;

            if (newProject.EndDate == null && oldProject.EndDate != null)
                newProject.EndDate = oldProject.EndDate;

            if (newProject.Team == null && oldProject.Team != null)
                newProject.Team = TimeUnit.Teams.Get(oldProject.Team.Id);

            if (newProject.TeamId == null)
                newProject.TeamId = newProject.Team.Id;

            if (newProject.CustomerId == 0)
                newProject.CustomerId = newProject.Customer.Id;

            if (newProject.Amount == null && oldProject.Amount != null)
                newProject.Amount = oldProject.Amount;
            return newProject;
        }
    }
}
