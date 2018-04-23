using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using TimeKeeper.API.Controllers;
using TimeKeeper.API.Models;
using System.Web.Http;
using System.Web;
using System.IO;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.Test.ControllerTests
{
    [TestClass]
    public class ControllerEmployeeTest
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetEmployeeByValidId()
        {
            var controller = new EmployeesController();
            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetEmployeeInvalidId()
        {
            var controller = new EmployeesController();
            var response = controller.GetById(25);
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerGetEmployeeByInvalidId()
        {
            var controller = new EmployeesController();

            var response = controller.GetById(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void ControllerGetAllEmployeesSuccess()
        {
            var controller = new EmployeesController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<EmployeeModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostEmployeeValid()
        {
            var controller = new EmployeesController();

            Employee employee = new Employee()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                BirthDate = DateTime.Now.AddYears(-32),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-1),
                Salary = 2200,
                Roles = new Role() { Id="TL" }
            };

            var response = controller.Post(employee);
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutEmployeeValid()
        {
            var controller = new EmployeesController();
            var response = controller.Put(new Employee()
            {
                Id = 4,
                FirstName = "Johnnnn",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                BirthDate = DateTime.Now.AddYears(-32),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-1),
                Salary = 2200,
                Roles = new Role() { Id="TL"}
            }, 4);

            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutEmployeeInvalidId()
        {
            var controller = new EmployeesController();
            var response = controller.Put(new Employee()
            {
                Id = 1337,
                FirstName = "Johnh",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                BirthDate = DateTime.Now.AddYears(-32),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-1),
                Salary = 2200,
                Roles = new Role() { Id = "TL" }
            }, 1337);

            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteEmployeeWithValidId()
        {
            var controller = new EmployeesController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteEmployeeWithInvalidID()
        {
            var controller = new EmployeesController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);

        }

    }
}
