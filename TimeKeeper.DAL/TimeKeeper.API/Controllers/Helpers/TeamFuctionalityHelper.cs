using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers
{
    public class TeamFuctionalityHelper : BaseController, IFunctionalityHelpers<Team, TeamModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }
        public bool Flag { get; set; }


        public IQueryable<Team> Filter(IQueryable<Team> query, string searchBy, string filter)
        {
            IQueryable<Team> filterQuery;
            switch (searchBy)
            {
                case "name":
                    filterQuery = query.Where(e => e.Name.Contains(filter));
                    break;
                case "desc":
                    filterQuery = query.Where(e => e.Description.Contains(filter));
                    break;
                default:
                    filterQuery = query;
                    break;
            }

            //if (filter != "") query = query.Where(e => e.LastName.Contains(filter));
            return filterQuery;
        }

        public List<TeamModel> Paginate(IQueryable<Team> query, int pageSize, int page)
        {
            List<TeamModel> paginateList;
            ItemCount = TimeUnit.Teams.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            paginateList = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t, false))
                                          .ToList();
            return paginateList;
        }

        public IQueryable<Team> Sort(IQueryable<Team> query, int sort)
        {
            IQueryable<Team> sortQuery;
            switch (sort)
            {
                case 1: sortQuery = query.OrderBy(x => x.Name); break;
                case 2: sortQuery = query.OrderBy(x => x.Description); break;
                default: sortQuery = query.OrderBy(x => x.Id); break;
            }
            return sortQuery;
        }
    }
}