using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public class Engagement : BaseClass<int>
    {
        public virtual Employee Employee { get; set; }
        public virtual Team Team { get; set; }
        public virtual Role Role { get; set; }

        [Required]
        public decimal Hours { get; set; }

    }
}
