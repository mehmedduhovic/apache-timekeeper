using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.OAuth
{
    public class TimeUserService : UserServiceBase
    {
        private readonly UnitOfWork unitOfWork;
        public TimeUserService(UnitOfWork unit)
        {
            unitOfWork = unit;
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = unitOfWork.Employees.Get().Where(x => x.Email == context.UserName && x.Password == context.Password).FirstOrDefault();
            if (user == null)
                context.AuthenticateResult = new AuthenticateResult("Bad username or password");
            else
                context.AuthenticateResult = new AuthenticateResult(context.UserName, context.Password);
        }
    }
}