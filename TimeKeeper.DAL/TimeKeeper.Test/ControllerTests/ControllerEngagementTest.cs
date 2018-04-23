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
    public class ControllerEngagementTest
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetEngagementSuccess()
        {
            var controller = new EngagementsController();
            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<EngagementModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetEngagementInvalidId()
        {
            var controller = new EngagementsController();
            var response = controller.GetById(25);
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerGetAllEngagementSuccess()
        {
            var controller = new EngagementsController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<EngagementModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPostEngagementValid()
        {
            var controller = new EngagementsController();

            Engagement engagement = new Engagement()
            {              
                Team = new Team() { Id = "A" },
                Role=new Role() { Id="TL"},
                Employee=new Employee() { Id = 1 },
                Hours = 4
            };

            var response = controller.Post(engagement);
            var result = (OkNegotiatedContentResult<EngagementModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutEngagementValid()
        {
            var controller = new EngagementsController();

            var response = controller.Put(new Engagement()
            {
                Id = 3,
                Team = new Team() { Id="A"},
                Role = new Role() { Id = "TL" },
                Employee = new Employee() { Id = 1 },
                Hours = 30
            }, 3);

            var result = (OkNegotiatedContentResult<EngagementModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutEngagementInvalidId()
        {
            var controller = new EngagementsController();

            var response = controller.Put(new Engagement()
            {
                Id = 1337,
                Team = new Team() { Id = "A" },
                Role = new Role() { Id = "TL" },
                Employee = new Employee() { Id = 1 },
                Hours = 30
            }, 1337);

            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteEngagementWithValidID()
        {
            var controller = new EngagementsController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteEngagementWithInvalidID()
        {
            var controller = new EngagementsController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);
        }
    }
}
