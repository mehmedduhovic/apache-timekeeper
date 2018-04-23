using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using TimeKeeper.API.Controllers;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.Test.ControllerTests
{
    [TestClass]
    public class ControllerProjectTest
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetProjectSuccess()
        {
            var controller = new ProjectsController();
            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<ProjectModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetProjectInvalidId()
        {
            var controller = new ProjectsController();
            var response = controller.GetById(25);
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerGetAllProjectsSuccess()
        {
            var controller = new ProjectsController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<ProjectModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostProjectValid()
        {
            var controller = new ProjectsController();


            Project pro = new Project()
            {
                Name = "Gigi School Of Coding",
                Description = "Gigi's Project",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Status = ProjectStatus.Finished,
                Pricing = PricingStatus.HourlyRate,
                Customer = new Customer() { Id = 1},
                Team = new Team() { Id = "A"}                
            };

            var response = controller.Post(pro);
            var result = (OkNegotiatedContentResult<ProjectModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }


        //[TestMethod]
        //public void ControllerPutProjectValid()
        //{
        //    var controller = new ProjectsController();
        //    var response = controller.Put(new ProjectModel()
        //    {
        //        Id = 1,
        //        Name = "Gigi School Of Coding",
        //        Description = "Gigi's Project",
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now,
        //        Status = ProjectStatus.Finished.ToString(),
        //        Pricing = PricingStatus.HourlyRate.ToString(),
        //        Customer=1,
        //        Team = "A"
        //    }, 1);

        //    var result = (OkNegotiatedContentResult<ProjectModel>)response;

        //    Assert.IsNotNull(result);
        //    Assert.IsNotNull(result.Content);
        //}

        //[TestMethod]
        //public void ControllerPutProjectInvalidId()
        //{
        //    var controller = new ProjectsController();
        //    var response = controller.Put(new Project()
        //    {
        //        Id = 1337,
        //        Name = "Gigi School Of Coding",
        //        Description = "Gigi's Project",
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now,
        //        Status = ProjectStatus.Finished,
        //        Pricing = PricingStatus.HourlyRate,
        //        Customer = new Customer() { Id = 1 },
        //        Team = new Team() { Id = "A" }
        //    }, 1337);

        //    var result = (NotFoundResult)response;

        //    Assert.IsNotNull(result);
        //}

        [TestMethod]
        public void ControllerDeleteProjectWithValidID()
        {
            var controller = new ProjectsController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteProjectWithInvalidID()
        {
            var controller = new ProjectsController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
