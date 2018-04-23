using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.Test
{
    [TestClass]
    public class ProjectTest
    {
        //[TestMethod]
        public void GetProjectFromDatabase_CheckTeam_ExistingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            Project NasaProject = context.Projects.FirstOrDefault(x => x.Name == "NASA Project");

            Assert.AreEqual("A", NasaProject.Team.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void AddProjects_WithoutHours_ValidationTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            Project newProject = new Project()
            {
                Name = "IMT Rezervni Dijelovi za Bagere",
                Description = "Some IMT Project",
                Status = ProjectStatus.Finished,
                Pricing = PricingStatus.HourlyRate,
                Team = context.Teams.Find("A")
            };

            context.Projects.Add(newProject);
            context.SaveChanges();
        }

        [TestMethod]
        public void CheckProject_WithNoEndDate_FailingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            var NasaProjectEndDate = context.Projects.FirstOrDefault(x => x.Name == "NASA Project").EndDate;

            //Assert
            Assert.AreEqual(null, NasaProjectEndDate);
        }
        /*
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ProjectSoftDelete_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();

            context.Projects.Remove(context.Projects.First(x => x.Name == "IMT Rezervni Dijelovi za Bagere"));
            context.SaveChanges();

            Project firstEmpo = context.Projects.First(x => x.Name == "IMT Rezervni Dijelovi za Bagere");
        }
        */
    }
}
