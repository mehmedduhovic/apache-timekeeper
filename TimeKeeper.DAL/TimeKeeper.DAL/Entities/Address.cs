using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public class Address
    {
        public Address() { }

        public string Road { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

    }
}