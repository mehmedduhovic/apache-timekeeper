using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Controllers.Helpers.ReportsHelpers;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.API.Reports
{
    public class ReportFactory
    {
        private UnitOfWork TimeUnit;

        public ReportFactory(UnitOfWork _unit)
        {
            TimeUnit = _unit;
        }
        /*
        public List<MissingEntriesModel> GetMissingEntries(int year, int month)
        {
            List<MissingEntriesModel> missingEntriesList = new List<MissingEntriesModel>();
            var employees = TimeUnit.Days.GetList(x => x.Date.Month == month && x.Date.Year == year)
                .Select(x => x.Employee).ToList();

            foreach(var employee in employees)
            {
                List<int> workingDays = DateTimeHelper.ListOfWorkingDays(year, month, employee.BeginDate.Day).ToList();
                var employeeDays = employee.Days.Where(y => y.Date.Month == month && y.Date.Year == year && y.Type == DAL.Entities.DayType.WorkingDay)
                    .Select(x => x.Date.Day).ToList();

                var missing = workingDays.Except(employeeDays).ToList();


                MissingEntriesModel emp = new MissingEntriesModel()
                {
                    Employee = employee.FirstName + " " + employee.LastName,
                    MissingDayCount = Math.Abs(TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id && x.Date.Year == year && x.Date.Month == month).Count() - workingDays.Count()),
                    MissingDays = missing
                };

                missingEntriesList.Add(emp);
            }

            return missingEntriesList;
        }
        */
        public List<AnnualModel> AnnualReport(int year)
        {
            var query = TimeUnit.Projects.Get()
                                .OrderBy(x => x.Name)
                                .Select(x => new
                                {
                                    project = x.Name,
                                    description = x.Description,
                                    count = x.Tasks.Count(t => t.Project.Name == x.Name),
                                    details = x.Tasks.Where(d => d.Day.Date.Year == year)
                                                     .GroupBy(d => d.Day.Date.Month)
                                                     .Select(w => new
                                                     {
                                                         month = w.Key,
                                                         hours = w.Sum(d => d.Hours)
                                                     }).ToList(),
                                }).ToList();

            List<AnnualModel> list = new List<AnnualModel>();
            AnnualModel total = new AnnualModel() { ProjectName = "TOTAL" };
            foreach(var q in query)
            {
                AnnualModel item = new AnnualModel() { ProjectName = q.project, ProjectDescription = q.description, TasksCount = q.count };
                foreach(var w in q.details)
                {
                    item.TotalHours += w.hours;
                    total.TotalHours += w.hours;
                    item.MonthlyHours[w.month - 1] = w.hours;
                    total.MonthlyHours[w.month - 1] += w.hours;
                }
                if (item.TotalHours > 0) list.Add(item);

            }
            list.Add(total);
            return list;
        }

        public MonthlyModel MonthlyReport(int year, int month)
        {
            MonthlyModel result = new MonthlyModel() { Year = year, Month = month };

            result.Projects = TimeUnit.Projects.Get().OrderBy(x => x.Monogram).Select(x => new ProjectItem
            {
                Project = x.Monogram,
                Hours = x.Tasks.Where(d => d.Day.Date.Year == year && d.Day.Date.Month == month).Sum(d => d.Hours)
            }).ToList();

            result.Projects.RemoveAll(q => q.Hours == null);
            result.Total = (decimal)result.Projects.Sum(x => x.Hours);
            int listSize = result.Projects.Count();

            var query = TimeUnit.Employees.Get().OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
                                .Select(x => new {
                                    employee = x.FirstName + " " + x.LastName,
                                    details = x.Days.Where(c => c.Date.Year == year && c.Date.Month == month)
                                                        .SelectMany(c => c.Tasks)
                                                        .OrderBy(p => p.Project.Monogram)
                                                        .GroupBy(p => p.Project.Monogram)
                                                        .Select(d => new { project = d.Key, hours = d.Sum(w => w.Hours) }).ToList()
                                }).ToList();
            foreach (var q in query)
            {
                MonthlyItem item = new MonthlyItem(listSize) { Employee = q.employee, Total = q.details.Sum(w => w.hours) };
                foreach (var d in q.details)
                {
                    int i = result.Projects.FindIndex(x => x.Project == d.project);
                    item.Hours[i] = d.hours;
                }
                if (item.Total != 0) result.Items.Add(item);
            }

            return result;
            }

        public ProjectHistoryModel ProjectHistory(int id)
        {
            decimal? monthlyRate = TimeUnit.Projects.Get(id).Amount;


            var project = TimeUnit.Projects.Get(id);
            List<int> years = new List<int>
            {
                2016,
                2017,
                2018
            };

            List<ProjectHistoryEmployee> ReportEmployees = new List<ProjectHistoryEmployee>();
            List<ProjectHistoryTotal> Total = new List<ProjectHistoryTotal>();
            var employees = TimeUnit.Projects.Get().Where(x => x.Id == id).Select(y => y.Team).SelectMany(z => z.Engagements).Select(y => y.Employee).ToList();
            List<int> ByYearTotal = new List<int>();
            foreach (var employee in employees)
            {
                decimal? employeeTotalHours = 0;
                ProjectHistoryEmployee empToAdd = new ProjectHistoryEmployee()
                {
                    Id = employee.Id,
                    Name = employee.FirstName + " " + employee.LastName
                };

                


                var totalHours = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id).SelectMany(y => y.Tasks).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                foreach (var year in years)
                {
                    var dailyHours = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id && x.Date.Year == year).GroupBy(x => new { x.Date.Month, x.Hours } ).Select(x => x.Sum(w => w.Hours)).ToList();
                    ProjectHistoryTotal empvar = new ProjectHistoryTotal()
                    {
                        DailyHours = dailyHours,
                        Year = year,
                        TotalHours = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id && x.Date.Year == year).SelectMany(y => y.Tasks).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum()
                    };
                    
                    empToAdd.Sums.Add(empvar);

                    employeeTotalHours += empvar.TotalHours;

                }
                empToAdd.TotalHours = employeeTotalHours;
                ReportEmployees.Add(empToAdd);

            }



            var projectTotalHours = TimeUnit.Days.Get().SelectMany(y => y.Tasks).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();

            foreach (var year in years)
            {
                ProjectHistoryTotal YearTotal = new ProjectHistoryTotal()
                {
                    Year = year,
                    TotalHours = TimeUnit.Days.Get().Where(x => x.Date.Year == year).SelectMany(y => y.Tasks).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum()
                };
                Total.Add(YearTotal);
            }


            return new ProjectHistoryModel()
            {
                Id = id,
                Name = project.Name,
                TotalHours = projectTotalHours,
                Employees = ReportEmployees,
                Amount = project.Amount,
                BeginDate = project.StartDate,
                EndDate = project.EndDate,
                Total = Total

            };
            /*
            decimal? monthlyRate = TimeUnit.Projects.Get(id).Amount;
            ProjectHistoryModel report = new ProjectHistoryModel();
            int yearTotal = 0;
            decimal? sumTotal = 0;

            var project = TimeUnit.Projects.Get(id);
            report.BeginDate = project.StartDate;
            report.EndDate = project.EndDate;
            report.ProjectName = project.Name;
            
            List<int> years = new List<int>
            {
                2015,
                2016,
                2017,
                2018
            };
            List<ProjectHistoryEmployee> ReportEmployees = new List<ProjectHistoryEmployee>();
            var employees = TimeUnit.Projects.Get().Where(x => x.Id == id).Select(y => y.Team).SelectMany(z => z.Engagements).Select(y => y.Employee).ToList();
            var engagements = TimeUnit.Projects.Get().Where(x => x.Id == id).Select(y => y.Team).SelectMany(z => z.Engagements);
            ProjectHistoryEmployee total = new ProjectHistoryEmployee { Name = "TOTAL" };
            total.TotalHours = 0;
            foreach (var employee in employees)
            {
                ProjectHistoryEmployee empToAdd = new ProjectHistoryEmployee();
                
                empToAdd.Id = employee.Id;
                empToAdd.Name = employee.FirstName + " " + employee.LastName;                

                var totalHours = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id).SelectMany(y => y.Tasks).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
                empToAdd.TotalHours = totalHours;
                total.TotalHours += empToAdd.TotalHours;
                foreach (var year in years)
                {
                    ProjectHistoryTotal empvar = new ProjectHistoryTotal()
                    {
                        Year = year,
                        Sum = TimeUnit.Days.Get().Where(x => x.Employee.Id == employee.Id && x.Date.Year == year).SelectMany(y => y.Tasks).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum()
                    };

                    ProjectHistoryTotal totalHis = new ProjectHistoryTotal()
                    {
                        Year = yearTotal,
                        Sum = sumTotal
                    };

                    yearTotal = year;
                    sumTotal += empvar.Sum;
                    
                    empToAdd.Sums.Add(empvar);
                    total.Sums.Add(totalHis);
                }
                
                
                sumTotal = 0;
                ReportEmployees.Add(empToAdd);
                
            }
            ReportEmployees.Add(total);
            report.Employees = ReportEmployees;


            var projectTotalHours = TimeUnit.Days.Get().SelectMany(y => y.Tasks).Where(x => x.Project.Id == id).Select(x => x.Hours).DefaultIfEmpty(0).Sum();
            report.TotalHours = projectTotalHours;
            return report;*/
        }


    }
}