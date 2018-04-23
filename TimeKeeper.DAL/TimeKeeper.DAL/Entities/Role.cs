using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public enum RoleType
    {
        JobTitle,
        RoleInTeam,
        RoleInApp
    }


    public class Role : BaseClass<string>
    {

        public Role()
        {
            Employees = new List<Employee>();
            Engagements = new List<Engagement>();

        }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        public RoleType Type { get; set; }
        public Decimal Hrate { get; set; }
        public Decimal Mrate { get; set; }

        public virtual ICollection<Engagement> Engagements { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

    }
}
