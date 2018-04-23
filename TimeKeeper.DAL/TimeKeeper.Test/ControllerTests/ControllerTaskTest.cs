using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using TimeKeeper.API.Controllers;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.Test.ControllerTests
{
    [TestClass]
    public class ControllerTaskTest
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetTaskByValidId()
        {
            var controller = new TasksController();
            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<TaskModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetTaskByInvalidId()
        {
            var controller = new TasksController();

            var response = controller.GetById(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void ControllerGetAllTasksSuccess()
        {
            var controller = new TasksController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<TaskModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetTaskInvalidId()
        {
            var controller = new TasksController();
            var response = controller.GetById(25);
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerPostTaskValid()
        {
            var controller = new TasksController();
            Task task = new Task()
            {
                Description = "Take Protein Pills",
                Hours = 4,
                Day = new Day() { Id=1},
                Project = new Project() { Id=1}
            };

            var response = controller.Post(task);
            var result = (OkNegotiatedContentResult<TaskModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutTaskValid()
        {
            var controller = new TasksController();

            var response = controller.Put(new Task()
            {
                Id = 1,
                Description = "Take Protein Pillsv2",
                Hours = 4,
                Day = new Day() { Id = 1 },
                Project = new Project() { Id = 1 }
            }, 1);

            var result = (OkNegotiatedContentResult<TaskModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutTaskInvalidId()
        {
            var controller = new TasksController();

            var response = controller.Put(new Task()
            {
                Id = 1337,
                Description = "Take Protein Pillsv2",
                Hours = 4,
                Day = new Day() { Id = 1 },
                Project = new Project() { Id = 1 }
            }, 1337);

            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteTaskWithValidID()
        {
            var controller = new TasksController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteTaskWithInvalidID()
        {
            var controller = new TasksController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }

    }
}
