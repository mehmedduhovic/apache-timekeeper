using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeKeeper.API.Controllers;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.Test.ControllerTests
{
    [TestClass]
    public class ControllerDayTest
    {/*
       [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetDayByValidId()
        {
            var controller = new DaysController();
            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<DayModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetDayInvalidId()
        {
            var controller = new DaysController();
            var response = controller.GetById(25);
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerGetDayByInvalidId()
        {
            var controller = new DaysController();

            var response = controller.GetById(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void ControllerGetAllDayssSuccess()
        {
            var controller = new DaysController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<DayModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostDayValid()
        {
            var controller = new DaysController();
 

            Day day = new Day()
            {
                Date = DateTime.Now.AddDays(-1),
                Type = DayType.WorkingDay,
                Hours = 8,
                Employee=new Employee() { Id=1}
            };

            var response = controller.Post(day);
            var result = (OkNegotiatedContentResult<DayModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public void ControllerPutDayValid()
        {

            var controller = new DaysController();
            var response = controller.Put(new Day()
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-1),
                Type = DayType.WorkingDay,
                Hours = 12,
                Employee = new Employee() { Id = 1 }
            }, 1);

            var result = (OkNegotiatedContentResult<DayModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutDayInvalidId()
        {

            var controller = new DaysController();
            var response = controller.Put(new Day()
            {
                Id = 1337,
                Date = DateTime.Now.AddDays(-1),
                Type = DayType.WorkingDay,
                Hours = 12,
                Employee = new Employee() { Id = 1 }
            }, 1337);

            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void ControllerDeleteDayWithValidId()
        {
            var controller = new DaysController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteDayWithInvalidId()
        {
            var controller = new DaysController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);
        }
        */

    }
}
