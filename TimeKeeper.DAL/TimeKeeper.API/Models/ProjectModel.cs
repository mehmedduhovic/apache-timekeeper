using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Monogram { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string Pricing { get; set; }
        public decimal? Amount { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string Team { get; set; }
        public string TeamId { get; set; }
    }
}