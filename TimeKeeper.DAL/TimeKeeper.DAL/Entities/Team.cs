using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public class Team : BaseClass<string>
    {
        public Team()
        {
            Engagements = new List<Engagement>();
            Projects = new List<Project>();
        }

        [Required, MaxLength(25)]
        public string Name { get; set; }

        public string Image { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Engagement> Engagements { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
