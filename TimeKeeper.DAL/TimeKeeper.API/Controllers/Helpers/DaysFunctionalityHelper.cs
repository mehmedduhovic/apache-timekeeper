using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers
{
    public class DaysFunctionalityHelper : BaseController, IFunctionalityHelpers<Day, DayModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }
        public bool Flag { get; set; }

        public IQueryable<Day> Filter(IQueryable<Day> query, string searchBy, string filter)
        {
            IQueryable<Day> filterQuery = query;
            return filterQuery;
        }

        public List<DayModel> Paginate(IQueryable<Day> query, int pageSize, int page)
        {
            List<DayModel> paginateList;
            ItemCount = TimeUnit.Days.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            //paginateList = query.Skip(pageSize * page)
            //                              .Take(pageSize)
            //                              .ToList()
            //                              .Select(t => TimeFactory.Create(t))
            //                              .ToList();
            return null;//paginateList;
        }

        public IQueryable<Day> Sort(IQueryable<Day> query, int sort)
        {
            IQueryable<Day> sortQuery;
            switch (sort)
            {
                case 1: sortQuery = query.OrderBy(x => x.Date); break;
                case 2: sortQuery = query.OrderBy(x => x.Hours); break;
                default: sortQuery = query.OrderBy(x => x.Id); break;
            }
            return sortQuery;
        }
    }
}