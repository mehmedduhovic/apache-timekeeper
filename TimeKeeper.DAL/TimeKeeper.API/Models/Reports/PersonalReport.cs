using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models.Reports
{
    public class PersonalReport
    {
        public EmployeeModel Employee { get; set; }
        public List<PersonalReportsDay> Days { get; set; }
        public decimal OvertimeHours { get; set; }
        public int MissingEntries { get; set; }
        public int WorkingDays { get; set; }
        public double PercentageOfWorkingDays { get; set; }
        public int VacationDays { get; set; }
        public int BussinessAbsenceDays { get; set; }
        public int PublicHolidayDays { get; set; }
        public int SickLeaveDays { get; set; }
        public int ReligiousDays { get; set; }
        public int OtherAbsenceDays { get; set; }
        public int NonWorkingDaysTotal { get; set; }
        public List<PersonalReportDistinct> DistinctProjects { get; set; }
    }

}