using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.DAL.Repositories
{

    class EmployeeRepository: Repository<Employee, int>

    {
        public EmployeeRepository(TimeKeeperDbContext context) : base(context) { }

        public override void Update(Employee entity, int id)
        {
            Employee old = Get(id);
            if(old != null)
            {
                context.Entry(old).CurrentValues.SetValues(entity);
                old.Roles = entity.Roles;
            }
        }
    }
}
