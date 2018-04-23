using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class UserModelTeam
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RoleModel Role { get; set; }
    }
}