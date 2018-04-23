using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class TeamModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<EngagementModel> Members { get; set; }
        public string Description { get; set; }
    }

    public class TeamDetailsModel : TeamModel
    {
        public string Image { get; set; }
        public ICollection<ProjectModel> Projects { get; set; }
    }
}