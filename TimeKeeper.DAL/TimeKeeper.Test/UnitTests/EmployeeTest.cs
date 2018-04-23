using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.Test
{
    [TestClass]
    public class EmployeeTest
    {

        [TestMethod]
        public void AddEmployee_WithCorrectInputs_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();

            Employee Fatima = new Employee()
            {
                FirstName = "Fatima",
                LastName = "Sinanovic",
                Email = "fatimasin@gmail.com",
                BirthDate = DateTime.Now.AddYears(-21),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-3),
                Salary = 1500,
                Roles = context.Roles.Find("TL")
            };

            context.Employees.Add(Fatima);
            context.SaveChanges();

            Assert.AreEqual(3, Fatima.Id);

        }
/*
        [TestMethod]
        public void AddEmployee_WithMissingFirstName_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            Employee Fatima = new Employee()
            {
                LastName = "Sinanovic",
                Email = "fatimasin@gmail.com",
                BirthDate = DateTime.Now.AddYears(-21),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-3),
                Salary = 1500,
                Roles = context.Roles.Find("TL")
            };

            var validationContext = new ValidationContext(Fatima, null, null);
            var results = new List<ValidationResult>();

            var result = Validator.TryValidateObject(Fatima, validationContext, results, true);

            foreach(var validationResult in results)
            {
                Console.WriteLine($"\n{validationResult.ErrorMessage}");
                Console.WriteLine();
            }

            Assert.IsFalse(result);
        }
        */
        /*
        [TestMethod]
        public void AddEmployee_WithMissingLastName_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            Employee Fatima = new Employee()
            {
                FirstName = "Fatima",
                Email = "fatimasin@gmail.com",
                BirthDate = DateTime.Now.AddYears(-21),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-3),
                Salary = 1500,
                Roles = context.Roles.Find("TL")
            };

            var validationContext = new ValidationContext(Fatima, null, null);
            var results = new List<ValidationResult>();

            var result = Validator.TryValidateObject(Fatima, validationContext, results, true);

            foreach (var validationResult in results)
            {
                Console.WriteLine($"\n{validationResult.ErrorMessage}");
                Console.WriteLine();
            }

            Assert.IsFalse(result);
        }
        */
        [TestMethod]
        public void CheckId_OfEmployee_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();

            Employee EdoRole = (Employee)context.Employees.Find(2);

            Assert.AreEqual("TL", EdoRole.Roles.Id);
        }

        [TestMethod]
        public void CheckSalary_OfEmployee_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();

            Employee E = (Employee)context.Employees.Find(2);

            Assert.AreEqual(6000, E.Roles.Mrate);
        }

        [TestMethod]
        public void CheckEmployees_WithSameRoles_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            int counter = context.Employees.Count();
            Employee E;
            int QAcounter = 0;
            for (int i=0; i<counter; i++)
            {
                E = (Employee)context.Employees.Find(i + 1);
                
                if(E.Roles.Id == "TL")
                {
                    QAcounter++;
                }
            }
            Assert.AreEqual(3, QAcounter);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmployeeSoftDelete_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            
            context.Employees.Remove(context.Employees.First(x => x.FirstName == "Fatima"));
            context.SaveChanges();
            Employee fatima = context.Employees.First(x => x.FirstName == "Fatima");
        }       
    }

}
