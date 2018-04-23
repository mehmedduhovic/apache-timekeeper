using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Monogram { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Address_City { get; set; }
        public string Address_ZipCode { get; set; }
        public string Address_Road { get; set; }
    }

    public class CustomerDetailsModel : CustomerModel
    {
        public ICollection<ProjectModel> Projects { get; set; }
    }
}