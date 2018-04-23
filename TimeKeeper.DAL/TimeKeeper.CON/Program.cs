using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.DAL.Repositories;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL;

namespace TimeKeeper.CON
{
    class Program
    {
        public class Result
        {
            public Employee Name { get; set; }
            public decimal TotalHours { get; set; }
            public decimal AverageHours { get; set; }

        }

        static void Main(string[] args)
        {
            // using (UnitOfWork unit = new UnitOfWork())
            // {
            /*
            var query = unit.Days.Get()
                .OrderBy(x => x.Employee.LastName);
            var query2 = query.Where(x => x.Type == DayType.SickLeave).GroupBy(x => x.Employee)
                .OrderBy(x => x.Key.LastName)
                .Select(w => new
                {
                    FirstName = w.Key.FirstName,
                    LastName = w.Key.LastName,
                    days = w.Count()
                });

            var list = query2.ToList();
            foreach (var empl in list)
            {
                Console.WriteLine($"{empl.LastName}, {empl.FirstName} = {empl.days}");


            var list = unit.Days.Get()
                .Where(x => x.Type == DayType.WorkingDay)
                .GroupBy(x => x.Employee)
                .OrderBy(x => x.Key.LastName)
                .Select(w => new Result
                {
                    Name = w.Key,
                    TotalHours = w.Sum(c => c.Hours),
                    AverageHours = w.Average(c => c.Hours)
                })
                .ToList();
            */
            /*
            var list = unit.Days.Get()
                .SelectMany(t => t.Tasks)
                .GroupBy(t => new { t.Project, t.Day.Employee })
                .Select(x => new {
                    project = x.Key,
                    hours = x.Sum(y => y.Hours),
                    avgh = x.Average(e => e.Hours)
                })
                .ToList();
                */
            //foreach (var time in list)
            //  {
            //    Console.WriteLine($"{time.project.Employee.LastName}, {time.hours}");
            // }

            /*int year, month;
            Console.Write("Please enter the year: ");
            year = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter the month: ");
            month = Convert.ToInt32(Console.ReadLine());
            int baze = DateTime.DaysInMonth(year, month);
            var query = unit.Employees.Get()
                .SelectMany(e => e.Days)
                .Where(d => d.Date.Month == month && d.Date.Year == year && d.Type == DayType.WorkingDay)
                .GroupBy(x => x.Employee)
                .Select(w => new { w.Key, wd = w.Count() })
                .ToList();

            foreach (var emp in query)
            {
                Console.WriteLine($"{emp.Key.LastName}, {emp.Key.FirstName} : {Math.Round((100m * emp.wd / baze), 2)}%");
            }
        }
        */

            double time;
            int count, result;
            using (TimeKeeperDbContext context = new TimeKeeperDbContext())
            {
                Console.Write("Enter task duration to search: ");
                decimal taskTime = Convert.ToDecimal(Console.ReadLine());
                count = 0;
                result = 0;
                DateTime srcStart = DateTime.Now;
                var details = context.Tasks
                    .Where(x => x.Hours == taskTime)
                    .Select(x => new { id = x.Id, project = x.Project.Name, desc = x.Description});
                foreach(var detail in details)
                {
                    count++;
                    result++;
                    Console.WriteLine($"{detail.id}: {detail.project} | {detail.desc}");
                }

                time = Math.Round((DateTime.Now - srcStart).TotalSeconds, 3);
            }


            Console.WriteLine("\n--------------------------------------------------");
            Console.WriteLine($"\n{count} records retrieved.");
            Console.WriteLine($"\n{result} records found.");
            Console.WriteLine($"\ntook {time} seconds to get it done.");
            Console.WriteLine("\n****************Press Any Key********************");
            Console.ReadLine();
        }
    }
}
