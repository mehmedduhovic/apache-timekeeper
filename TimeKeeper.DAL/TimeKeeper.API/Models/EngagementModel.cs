using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class EngagementModel
    {
        public int Id { get; set; }
        public RoleModel Team { get; set; }
        public RoleModel Role { get; set; }
        public BaseModel Employee { get; set; }
        public decimal Hours { get; set; }
    }

    public class EngagementDetailsModel : EngagementModel
    {

    }
}