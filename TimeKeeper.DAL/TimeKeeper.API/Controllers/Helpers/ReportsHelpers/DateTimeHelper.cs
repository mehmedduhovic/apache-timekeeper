using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Controllers.Helpers.ReportsHelpers
{
    public static class DateTimeHelper
    {
        public static IEnumerable<int> ListOfWorkingDays(int year, int month, int day = 1)
        {
            DateTime start = new DateTime(year, month, day);
            DateTime stop = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            List<int> days = new List<int>();

            while(start <= stop)
            {
                if(start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    days.Add(start.Day);
                }
                start = start.AddDays(1);
            }
            return days;
        }
    }
}