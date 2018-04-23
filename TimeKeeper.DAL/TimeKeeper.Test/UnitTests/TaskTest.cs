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
    public class TaskTest
    {
        [TestMethod]
        public void NumberOfTasks_ByProject_CountTest()
        {

            TimeKeeperDbContext context = new TimeKeeperDbContext();
            ICollection<DAL.Entities.Task> listOfTasks = context.Tasks.ToList();
            int countNasaTasks = 0;
            foreach(var task in listOfTasks)
            {
                if(task.Project.Name == "NASA Project")
                {
                    countNasaTasks++;
                }
            }

            Assert.AreEqual(2, countNasaTasks);
        }

        [TestMethod]
        public void NameOfTask_OfProject_NameTest()
        {

            TimeKeeperDbContext context = new TimeKeeperDbContext();
            DAL.Entities.Task EngineOnTask = context.Tasks.Find(2);

            Assert.AreEqual("Commencing Countdown", EngineOnTask.Description);
        }

        /*
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TaskSoftDelete_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();

            context.Tasks.Remove(context.Tasks.First(x => x.Description == "Take Protein Pills2"));
            context.SaveChanges();

            DAL.Entities.Task TakeProteinPills = context.Tasks.First(x => x.Description == "Take Protein Pills2");
        }
        */
    }
}
