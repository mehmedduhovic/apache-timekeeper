using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class TeamMemberModel
    {

        public BaseModel Employee { get; set; }
        public decimal? TotalHours { get; set; }
        public DayStatisticModel Days { get; set; }
    }
}