using System;
using System.Collections.Generic;
using System.Linq;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers
{

    public class EmployeeFunctionalityHelper : BaseController, IFunctionalityHelpers<Employee,EmployeeModel>

    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }
        public bool Flag { get; set; }


        public IQueryable<Employee> Sort(IQueryable<Employee> query, int sort)
        {
            IQueryable<Employee> sortQuery;
            
            switch (sort)
            {
                case 1: sortQuery = query.OrderBy(x => x.LastName); break;
                case 2: sortQuery = query.OrderByDescending(x => x.LastName); break;
                default: sortQuery = query.OrderBy(x => x.Id); break;
            }
            return sortQuery;
        }

        public List<EmployeeModel> Paginate(IQueryable<Employee> query, int pageSize, int page)
        {
            List<EmployeeModel> paginateList;
            ItemCount = TimeUnit.Employees.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            paginateList = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();
            return paginateList;
        }

        public IQueryable<Employee> Filter(IQueryable<Employee> query, string searchBy, string filter)
        {
            IQueryable<Employee> filterQuery;
            switch(searchBy)
            {
                case "firstname":
                    filterQuery = query.Where(e => e.FirstName.Contains(filter));
                    break;
                case "lastname":
                    filterQuery = query.Where(e => e.LastName.Contains(filter));
                    break;
                default:
                    filterQuery = query;
                    break;
            }

            //if (filter != "") query = query.Where(e => e.LastName.Contains(filter));
            return filterQuery;
        }
    }
}