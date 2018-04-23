using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class ProjectHistoryModel
    {
        public ProjectHistoryModel()
        {
            Employees = new List<ProjectHistoryEmployee>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TotalHours { get; set; }
        //public List<AnnualReportEmployees> Employees { get; set; }
        public List<ProjectHistoryEmployee> Employees { get; set; }
        public List<ProjectHistoryTotal> Total { get; set; }
    }

    public class ProjectHistoryTotal
    {
        public int Year { get; set; }
        public decimal? TotalHours { get; set; }
        public List<decimal> DailyHours { get; set; }
    }
    public class ProjectHistoryEmployee
    {
        public ProjectHistoryEmployee()
        {
            Sums = new List<ProjectHistoryTotal>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectHistoryTotal> Sums { get; set; }
        public decimal? TotalHours { get; set; }
        
    }
}