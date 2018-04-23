using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class CompanyReportTeams
    {

        public string TeamName { get; set; }
        public decimal? OvertimeHours { get; set; }
    }
}