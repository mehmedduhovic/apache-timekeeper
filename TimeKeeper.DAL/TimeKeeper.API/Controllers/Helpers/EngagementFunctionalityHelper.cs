using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers
{
    public class EngagementFunctionalityHelper : BaseController, IFunctionalityHelpers<Engagement, EngagementModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }
        public bool Flag { get; set; }


        public IQueryable<Engagement> Sort(IQueryable<Engagement> query, int sort)
        {
            IQueryable<Engagement> sortQuery;
            switch (sort)
            {
                default: sortQuery = query.OrderBy(x => x.Id); break;
            }
            return sortQuery;
        }

        public List<EngagementModel> Paginate(IQueryable<Engagement> query, int pageSize, int page)
        {
            List<EngagementModel> paginateList;
            ItemCount = TimeUnit.Engagements.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            paginateList = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();
            return paginateList;
        }

        public IQueryable<Engagement> Filter(IQueryable<Engagement> query, string searchBy, string filter)
        {
            IQueryable<Engagement> filterQuery;
            switch (searchBy)
            {
                case "name":
                    filterQuery = query.Where(e => e.Team.Name.Contains(filter));
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