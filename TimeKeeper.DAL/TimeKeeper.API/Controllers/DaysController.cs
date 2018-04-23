using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.API.Controllers.Helpers.Validations;
using TimeKeeper.API.Models;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers
{
    public class DaysController : BaseController

    {   /// <summary>
        /// Get all Days.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 
        [Route("api/calendar/days/{id}/{year}/{month}")]
        public IHttpActionResult Get(int id, int year = 0, int month = 0)
        {
            if (year == 0) year = DateTime.Today.Year;
            if (month == 0) month = DateTime.Today.Month;
            Employee emp = TimeUnit.Employees.Get(id);
            CalendarModel calendar = new CalendarModel(new BaseModel { Id = emp.Id, Name = emp.FirstName + " " + emp.LastName }, year, month);
            var days = emp.Days.Where(x => x.Date.Month == month && x.Date.Year == year).ToList();
            int i;
            foreach (var day in days)
            {
                i = day.Date.Day - 1;
                calendar.Days[i].Id = day.Id;
                calendar.Days[i].Type = (int)day.Type;
                calendar.Days[i].Hours = day.Hours;
                calendar.Days[i].Details = day.Tasks.Select(x => TimeFactory.Create(x)).ToArray();
            }

            Utility.Log("DAY CONTROLLER: Get() Called.", "INFO");
            return Ok(calendar);
        }

        /// <summary>
        /// Insert Day.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly-created item</response>
            public IHttpActionResult Post([FromBody] DayModel model)
        {
            try
            {
                Day day = new Day
                {
                    Id = model.Id,
                    Date = model.Date,
                    Type = (DayType)model.Type,
                    Hours = model.Hours,
                    Employee = TimeUnit.Employees.Get(model.Employee.Id)
                };
                if (day.Id == 0)
                    TimeUnit.Days.Insert(day);
                else
                    TimeUnit.Days.Update(day, day.Id);
                TimeUnit.Save();

                foreach (TaskModel task in model.Details)
                {
                    if (task.Deleted)
                    {
                        TimeUnit.Tasks.Delete(task.Id);
                    }
                    else
                    {
                        Task detail = new Task
                        {
                            Id = task.Id,
                            Day = TimeUnit.Days.Get(day.Id),
                            Description = task.Description,
                            Hours = task.Hours,
                            Project = TimeUnit.Projects.Get(task.Project.Id)
                        };
                        if (detail.Id == 0)
                            TimeUnit.Tasks.Insert(detail);
                        else
                            TimeUnit.Tasks.Update(detail, detail.Id);
                    }
                }
                TimeUnit.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}



