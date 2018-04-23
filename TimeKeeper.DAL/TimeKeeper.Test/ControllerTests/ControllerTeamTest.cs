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

namespace TimeKeeper.Test.ControllerTests
{
    [TestClass]
    public class ControllerTeamTest
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetTeamSuccess()
        {
            var controller = new TeamsController();
            var response = controller.GetById("A");
            var result = (OkNegotiatedContentResult<TeamModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetCustomerInvalidId()
        {
            var controller = new TeamsController();
            var response = controller.GetById("ohoho");
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerGetAllTeamSuccess()
        {
            var controller = new TeamsController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<TeamModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostTeamValid()
        {
            var controller = new TeamsController();

            Team t = new Team()
            {
                Id="AT",
                Name = "Apache Team",
                Image = "AT"
            };

            var response = controller.Post(t);
            var result = (OkNegotiatedContentResult<TeamModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutTeamValid()
        {
            var controller = new TeamsController();
            var response = controller.Put(new Team()
            {
                Id = "A",
                Name="Alpha111",
                Image="A"
            }, "A");

            var result = (OkNegotiatedContentResult<TeamModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public void ControllerPutTeamInvalidId()
        {
            var controller = new TeamsController();
            var response = controller.Put(new Team()
            {
                Id = "SKLJ",
                Name = "Alpha111",
                Image = "A"
            }, "SKLJ");

            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteTeamWithValidID()
        {
            var controller = new TeamsController();
            var response = controller.Delete("B");
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteTeamWithInvalidID()
        {
            var controller = new TeamsController();

            var response = controller.Delete("asdfgh");
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }
    }
}
