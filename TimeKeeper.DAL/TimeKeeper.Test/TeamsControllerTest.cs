using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeKeeper.API.Controllers;
using TimeKeeper.API.Models;

namespace TimeKeeper.Test
{
    [TestClass]
    public class TeamsControllerTest
    {
        [TestMethod]
        public void GetAllTeamsSuccess()
        {
            var controller = new TeamsController();
            var response = controller.Get();
            var result = (OkNegotiatedContentResult<List<TeamModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void GetTeamSuccess()
        {
            var controller = new TeamsController();
            var response = controller.Get("A");
            var result = (OkNegotiatedContentResult<TeamModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }
    }
}
