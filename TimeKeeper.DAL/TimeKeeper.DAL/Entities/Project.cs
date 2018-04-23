using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.DAL.Entities
{

    public enum ProjectStatus { InProgress, OnHold, Finished, Canceled };
    public enum PricingStatus { HourlyRate, PerCapitaRate, FixedPrice, NotBillable };


    public class Project : BaseClass<int>
    {


        public Project()
        {
            Tasks = new List<Task>();
        }

        public string Monogram { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public virtual Team Team { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public PricingStatus Pricing { get; set; }
        public decimal? Amount { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string TeamId { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }



    }
}
