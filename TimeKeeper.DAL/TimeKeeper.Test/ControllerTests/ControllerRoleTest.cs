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
    public class ControllerRoleTest
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetRoleSuccess()
        {
            var controller = new RolesController();
            var response = controller.GetById("TL");
            var result = (OkNegotiatedContentResult<RoleModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetCustomerInvalidId()
        {
            var controller = new RolesController();
            var response = controller.GetById("ohoho");
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerGetAllRoleSuccess()
        {
            var controller = new RolesController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<RoleModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostRoleValid()
        {
            var controller = new RolesController();

            Role r = new Role()
            {
                Id = "DEV",
                Name = "Developer test",
                Type = RoleType.JobTitle,
                Hrate = 30,
                Mrate = 4100,
            };

            var response = controller.Post(r);
            var result = (OkNegotiatedContentResult<RoleModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
        [TestMethod]
        public void ControllerPutRolesValid()
        {
            var controller = new RolesController();
            var response = controller.Put(new Role()
            {
                Id = "TL",
                Name = "Quality Assurance Engineer 222",
                Type = RoleType.JobTitle,
                Hrate =30,
                Mrate = 4100,
            }, "TL");

            var result = (OkNegotiatedContentResult<RoleModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutRolesInvalidId()
        {
            var controller = new RolesController();
            var response = controller.Put(new Role()
            {
                Id = "SKLJ",
                Name = "Quality Assurance Engineer 222",
                Type = RoleType.JobTitle,
                Hrate = 30,
                Mrate = 4100,
            }, "SKLJ");

            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteRoleWithValidID()
        {
            var controller = new RolesController();
            var response = controller.Delete("SD");
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteRoleWithInvalidID()
        {
            var controller = new RolesController();

            var response = controller.Delete("asdfgh");
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }

    }
}
