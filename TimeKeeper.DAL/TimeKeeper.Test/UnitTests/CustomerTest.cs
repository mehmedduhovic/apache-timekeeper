using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.Test.UnitTests
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void AddCustomer_ValidData_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            Customer Mistral = new Customer()
            {
                Name = "Mistral",
                Status = CustomerStatus.Client,
                Contact = "Mersed Camdzic",
                Email = "info@mistral.ba",
                Phone = "062 212 213"
            };

            context.Customers.Add(Mistral);
            context.SaveChanges();
            Assert.AreEqual(3, context.Customers.Count());
        }

        /*
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CustomerSoftDelete_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();

            context.Customers.Remove(context.Customers.FirstOrDefault(x => x.Name == "Mersed Camdzic"));
            context.SaveChanges();

            Customer Redzo = context.Customers.First(x => x.Name == "Mersed Camdzic");
        }
        */
    }
}
