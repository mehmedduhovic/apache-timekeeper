using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Salary { get; set; }
        public string Status { get; set; }
        public RoleModel Roles { get; set; }

    }

    public class EmployeeDetailsModel : EmployeeModel
    {
        public ICollection<DayModel> Days { get; set; }
        public ICollection<EngagementModel> Engagements { get; set; }
    }


}