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
    public class ControllerCustomerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            HttpContext.Current = new HttpContext(
           new HttpRequest("", "http://tempuri.org", ""),
           new HttpResponse(new StringWriter()));
        }

        [TestMethod]
        public void ControllerGetCustomerSuccess()
        {
            var controller = new CustomersController();
            var response = controller.GetById(1);
            var result = (OkNegotiatedContentResult<CustomerModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetCustomerInvalidId()
        {
            var controller = new CustomersController();
            var response = controller.GetById(25);
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerGetAllCustomerSuccess()
        {
            var controller = new CustomersController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<CustomerModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostCustomersSuccess()
        {
            Customer c = new Customer()
            {
                Name = "Maestral Solutions",
                Contact = "Sulejman Catibusic",
                Email = "info@mistral.ba",
                Phone = "+38761200333",
                Status = CustomerStatus.Client,
            };

            var controller = new CustomersController();
            var response = controller.Post(c);
            var result = (OkNegotiatedContentResult<CustomerModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutCustomersSuccess()
        {
            var controller = new CustomersController();
            var response = controller.Put(new Customer()
            {
                Id = 3,
                Name = "New maestral Solutions",
                Contact = "Sulejman2 Catibusic",
                Email = "info@mistral.ba",
                Phone = "+38761200333",
                Status = CustomerStatus.Client

            }, 3);
            var result = (OkNegotiatedContentResult<CustomerModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }


        [TestMethod]
        public void CotrollerPutCustomerWithInvalidId()
        {
            var controller = new CustomersController();
            var response = controller.Put(new Customer()
            {
                Id = 255,
                Name = "New maestral Solutions",
                Contact = "Sulejman2 Catibusic",
                Email = "info@mistral.ba",
                Phone = "+38761200333",
                Status = CustomerStatus.Client

            }, 255);
            var result = (NotFoundResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteCustomerWithValidId()
        {

            var controller = new CustomersController();
            var response = controller.Delete(2);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ControllerDeleteCustomerWithInvalidId()
        {
            var controller = new CustomersController();

            var response = controller.Delete(1337);
            var result = (NotFoundResult)response;
            Assert.IsNotNull(result);
        }
    }
}
