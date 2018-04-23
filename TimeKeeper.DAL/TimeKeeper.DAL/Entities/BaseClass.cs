using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.DAL.Entities
{
    public abstract class BaseClass<T>
    {
        public BaseClass()
        {
            CreatedBy = 0;
            CreatedOn = DateTime.UtcNow;
            Deleted = false;
        }

        public T Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
    }


}
