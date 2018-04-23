using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers
{
    public class RoleFunctionalityHelper : BaseController, IFunctionalityHelpers<Role, RoleModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }
        public bool Flag { get; set; }


        public IQueryable<Role> Filter(IQueryable<Role> query, string searchBy, string filter)
        {
            IQueryable<Role> filterQuery;
            switch (searchBy)
            {
                case "name":
                    filterQuery = query.Where(e => e.Name.Contains(filter));
                    break;
                //case "":
                //    filterQuery = query.Where(e => e..Contains(filter));
                //break;
                default:
                    filterQuery = query;
                    break;
            }

            //if (filter != "") query = query.Where(e => e.LastName.Contains(filter));
            return filterQuery;
        }

        public List<RoleModel> Paginate(IQueryable<Role> query, int pageSize, int page)
        {
            List<RoleModel> paginateList;
            ItemCount = TimeUnit.Roles.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            paginateList = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();
            return paginateList;
        }

        public IQueryable<Role> Sort(IQueryable<Role> query, int sort)
        {
            IQueryable<Role> sortQuery;
            switch (sort)
            {
                case 1: sortQuery = query.OrderBy(x => x.Type); break;
                case 2: sortQuery = query.OrderBy(x => x.Name); break;
                default: sortQuery = query.OrderBy(x => x.Id); break;
            }
            return sortQuery;
        }
    }

}