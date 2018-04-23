using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public class Task : BaseClass<int>
    {
        public virtual Day Day { get; set; }
        public virtual Project Project { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal Hours { get; set; }
    }
}
