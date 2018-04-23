using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.Test
{
    [TestClass]
    public class DayTest
    {
        [TestMethod]
        public void CheckTotalWorkDays_ForEmployee_CountTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            int TotalDays = context.Employees.FirstOrDefault(x => x.FirstName == "Edo").Days.Count();

            Assert.AreEqual(2, TotalDays);
        }

        [TestMethod]
        public void CheckTotalWorkHours_ForEmployee_CountTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            //ICollection<Day> EdosWorkingDays = context.Employees.FirstOrDefault(x => x.FirstName == "Edo").Days;
            var hours = context.Days.Where(x => x.Employee.FirstName == "Edo").Sum(x => x.Hours);
            //decimal TotalHours = 0M;



            Assert.AreEqual(16, hours);
        }

        [TestMethod]
        public void CheckForNonExistingDay_ForEmployee_FailingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            DateTime NonExistingDay = DateTime.Now.AddDays(-225);
            Boolean Check = false;
            ICollection<Day> EdosWorkingDays = context.Employees.FirstOrDefault(x => x.FirstName == "Edo").Days;
            foreach (var day in EdosWorkingDays)
            {
                if (day.Date == NonExistingDay)
                    Check = true;
            }

            Assert.IsFalse(Check);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CheckWorkDays_ForEmployeeThatHasNoDays_FailingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            ICollection<Day> FatimaWorkingDays = context.Employees.FirstOrDefault(x => x.FirstName == "Fatima").Days;

            //Assert
        }

        /*
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DaySoftDelete_PassingTest()
        {
            DateTime date = DateTime.Now.AddDays(-1);
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            context.Days.Remove(context.Days.FirstOrDefault(x => x.Date == date));
            context.SaveChanges();

            Day FirstDay = context.Days.FirstOrDefault(x => x.Date == date);

            Console.WriteLine(FirstDay);
        }    
        */
    }
}
