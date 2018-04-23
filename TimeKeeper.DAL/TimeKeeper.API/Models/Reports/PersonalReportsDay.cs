using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Models.Reports
{
    public class PersonalReportsDay
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Hours { get; set; }
        public decimal OvertimeHours { get; set; }
        public string Comment { get; set; }
        public virtual List<PersonalReportTask> Tasks{ get; set; }
    }
}