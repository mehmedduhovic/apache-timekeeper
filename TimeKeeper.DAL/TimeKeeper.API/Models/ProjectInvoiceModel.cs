using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Models
{
    public class ProjectInvoiceModel
    {
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TeamName { get; set; }
        public decimal? Amount { get; set; }
        public List<RoleInvoiceModel> Roles { get; set; }
        public string MailBody { get; set; }
    }

    public class RoleInvoiceModel
    {
        public string Description { get; set; }
        public decimal? Quanity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Subotal { get; set; }
    }

}