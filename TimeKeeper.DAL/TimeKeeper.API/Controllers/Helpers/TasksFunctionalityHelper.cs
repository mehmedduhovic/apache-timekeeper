using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers
{
    public class TasksFunctionalityHelper : BaseController, IFunctionalityHelpers<Task, TaskModel>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int ItemCount { get; set; }
        public bool Flag { get; set; }


        public IQueryable<Task> Filter(IQueryable<Task> query, string searchBy, string filter)
        {
            IQueryable<Task> filterQuery;
            switch (searchBy)
            {
                case "description":
                    filterQuery = query.Where(e => e.Description.Contains(filter));
                    break;
                default:
                    filterQuery = query;
                    break;
            }

            //if (filter != "") query = query.Where(e => e.LastName.Contains(filter));
            return filterQuery;
        }

        public List<TaskModel> Paginate(IQueryable<Task> query, int pageSize, int page)
        {
            List<TaskModel> paginateList;
            ItemCount = TimeUnit.Tasks.Get().Count();
            TotalPages = (int)Math.Ceiling((double)ItemCount / pageSize);

            paginateList = query.Skip(pageSize * page)
                                          .Take(pageSize)
                                          .ToList()
                                          .Select(t => TimeFactory.Create(t))
                                          .ToList();
            return paginateList;
        }

        public IQueryable<Task> Sort(IQueryable<Task> query, int sort)
        {
            IQueryable<Task> sortQuery;
            switch (sort)
            {
                case 1: sortQuery = query.OrderBy(x => x.Description); break;
                case 2: sortQuery = query.OrderBy(x => x.CreatedOn); break;
                default: sortQuery = query.OrderBy(x => x.Id); break;
            }
            return sortQuery;
        }
    }
}