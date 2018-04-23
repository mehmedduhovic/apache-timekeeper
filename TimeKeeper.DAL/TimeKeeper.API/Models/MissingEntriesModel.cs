using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class MissingEntriesModel
    {
        public EmployeeModel Employee { get; set; }
        public int MissingDaysCount { get; set; }
        public List<int> MissingDays { get; set; }
        public string MailBody { get; set; }
    }
}