using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Controllers.Helpers
{
    public class Header
    {
        public int NextPage { get; set; }
        public int PreviousPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int Sort { get; set; }
        public int TotalItems { get; set; }
        public bool Flag { get; set; }

        public Header(int pageSize, int totalPages, int page, int sort, int totalItems, bool flag)
        {
            NextPage = (page >= totalPages - 1) ? -1 : page + 1;
            PreviousPage = page - 1;
            PageSize = pageSize;
            TotalPages = totalPages;
            Page = page;
            Sort = sort;
            TotalItems = totalItems;
            Flag = flag;
        }

        public Header(int pageSize, int totalPages, int page, int sort, int totalItems)
        {
            NextPage = (page >= totalPages - 1) ? -1 : page + 1;
            PreviousPage = page - 1;
            PageSize = pageSize;
            TotalPages = totalPages;
            Page = page;
            Sort = sort;
            TotalItems = totalItems;
        }
    }
}