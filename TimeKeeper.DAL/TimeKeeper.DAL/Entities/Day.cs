using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{

    public enum DayType
    {
        WorkingDay = 1,
        PublicHoliday,
        OtherAbsence,
        ReligiousDay,
        SickLeave,
        Vacation,
        BusinessAbsence
    };

    public class Day : BaseClass<int>
    {
        public Day()
        {
            Tasks = new List<Task>();
        }

        //[Required]
        public DateTime Date { get; set; }
        public DayType Type { get; set; }

        //[Required]
        public decimal Hours { get; set; }
        public string Comment { get; set; }

        //[Required]
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
