using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class DayDetailsModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Ordinal { get; set; }
        public string TypeOfDay { get; set; }
        public decimal Hours { get; set; }
        public ICollection<TaskModel> Tasks { get; set; }
        public DayDetailsModel()
        {
            Tasks = new List<TaskModel>();
        }
    }
}