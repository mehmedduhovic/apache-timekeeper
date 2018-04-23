using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{

    public enum EmployeeStatus
    {
        Trial,
        Active,
        Leaver
    };

    public class Employee : BaseClass<int>
    {
        public Employee()
        {
            Days = new List<Day>();
            Engagements = new List<Engagement>();
        }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        public string Image { get; set; }

        [MaxLength(40)]
        public string Email { get; set; }



        public string Password { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BirthDate { get; set; }

        public EmployeeStatus? Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BeginDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [Range(typeof(decimal), "0", "99999")]
        public decimal? Salary { get; set; }

        public virtual Role Roles { get; set; }

        public virtual ICollection<Day> Days { get; set; }
        public virtual ICollection<Engagement> Engagements { get; set; }
    }
}
