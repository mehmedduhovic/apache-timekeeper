using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TimeKeeper.API.Models.Reports
{
    public class PersonalReportTask
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public decimal Hours { get; set; }
    }


    public class PersonalReportDistinct
    {
        public string Name { get; set; }
        public decimal TotalHours { get; set; }
    }
}