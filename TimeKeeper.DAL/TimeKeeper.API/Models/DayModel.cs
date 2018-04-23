using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Controllers;
using TimeKeeper.DAL.Entities;


namespace TimeKeeper.API.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Hours { get; set; }
        public BaseModel Project { get; set; }
        public bool Deleted { get; set; }
    }

    public class DayModel
    {
        public DayModel(BaseModel employee)
        {
            Employee = employee;
            Details = new TaskModel[0];
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public BaseModel Employee { get; set; }
        public int Type { get; set; }
        public decimal Hours { get; set; }
        public TaskModel[] Details { get; set; }
    }

    public class CalendarModel
    {
        public CalendarModel(BaseModel employee, int year, int month)
        {
            Employee = employee;
            Month = month;
            Year = year;
            int Limit = DateTime.DaysInMonth(year, month);
            Days = new DayModel[Limit];
            for (int i = 0; i < Limit; i++)
            {
                Days[i] = new DayModel(employee)
                {
                    Date = new DateTime(year, month, i + 1),
                    Hours = 0,
                    Type = 0
                };
                if (Days[i].Date >= DateTime.Today) Days[i].Type = 9;               // future
                if (Days[i].Date.DayOfWeek == DayOfWeek.Saturday ||
                    Days[i].Date.DayOfWeek == DayOfWeek.Sunday) Days[i].Type = 8;   // weekend
            }
        }
        public BaseModel Employee { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DayModel[] Days { get; set; }
    }
}