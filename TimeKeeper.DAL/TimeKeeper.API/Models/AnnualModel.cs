using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeKeeper.API.Models
{
    public class AnnualModel
    {

        public AnnualModel()
        {
            MonthlyHours = new decimal[12];
            Months = new AnnualMonthModel[12];
        }

        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public decimal TotalHours { get; set; }
        public int TasksCount { get; set; }
        public decimal[] MonthlyHours { get; set; }
        public AnnualMonthModel[] Months { get; set; }
    }

    public class AnnualMonthModel
    {
        public decimal[] Hours { get; set; }
    }


    public class Outer
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public decimal Sum { get; set; }
    }

    public class Inner
    {
        public string Name { get; set; }
        public List<Outer> Outer { get; set; }
    }
}