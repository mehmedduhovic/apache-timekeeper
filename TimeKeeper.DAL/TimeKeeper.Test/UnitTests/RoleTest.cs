using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.Test
{
    [TestClass]
    public class RoleTest
    {
        [TestMethod]
        public void CountRoles_PassingTest()
        {
            //Arrange
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            //Act
            int roles = context.Roles.Count();
            //Assert
            Assert.AreEqual(2, roles);
        }

        [TestMethod]
        public void CountHrate_OfRoles_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            int numberOfRoles = context.Roles.Count();
            IQueryable<Role> roles = context.Roles;
            int sameHrate = 0;
            foreach (var role in roles)
            {
                if (role.Hrate == 30)
                    sameHrate++;
            }
            Assert.AreEqual(1, sameHrate);
        }

        /*
        [TestMethod]
        public void RoleSoftDelete_PassingTest()
        {
            TimeKeeperDbContext context = new TimeKeeperDbContext();
            Role R = (Role)context.Roles.Find("QA");

            context.Roles.Remove(R);

            int undeleted = 0;
            int numberOfRoles = context.Roles.Count();
            for (int i = 1; i < numberOfRoles; i++)
            {
                if (R.Deleted == false)
                    undeleted++;
            }
            context.SaveChanges();
            Assert.AreEqual(4, undeleted);
        }
        */
    }
}
