using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class DayStatisticModel
    {
        public decimal? OvertimeHours { get; set; }
        public int MissingEntries { get; set; }
        public int WorkingDays { get; set; }
        public double PercentageOfWorkingDays { get; set; }
        public int VacationDays { get; set; }
        public int BusinessAbscenceDays { get; set; }
        public int PublicHolidays { get; set; }
        public int SickLeaveDays { get; set; }
        public int ReligiousDays { get; set; }
        public int OtherAbscenceDays { get; set; }
    }
}