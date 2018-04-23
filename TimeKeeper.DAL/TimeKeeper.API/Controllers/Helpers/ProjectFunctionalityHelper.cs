using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers
{
    public class ProjectFunctionalityHelper : BaseController, IFunctionalityHelpers<Project, ProjectModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }
        public bool Flag { get; set; }


        public IQueryable<Project> Sort(IQueryable<Project> query, int sort)
        {
            IQueryable<Project> sortQuery;
            switch (sort)
            {
                case 1: sortQuery = query.OrderBy(x => x.Name); break;
                case 2: sortQuery = query.OrderBy(x => x.StartDate); break;
                default: sortQuery = query.OrderBy(x => x.Id); break;
            }
            return sortQuery;
        }

        public List<ProjectModel> Paginate(IQueryable<Project> query, int pageSize, int page)
        {
            List<ProjectModel> paginateList;
            ItemCount = TimeUnit.Projects.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            paginateList = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();
            return paginateList;
        }

        public IQueryable<Project> Filter(IQueryable<Project> query, string searchBy, string filter)
        {
            IQueryable<Project> filterQuery;
            switch (searchBy)
            {
                case "name":
                    filterQuery = query.Where(e => e.Name.Contains(filter));
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