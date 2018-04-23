using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TimeKeeper.API.Models;
using TimeKeeper.API.Models.Reports;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.API.Controllers.Helpers.ReportsHelpers
{
    public static class ReportsExtensions
    {
        public static List<MissingEntriesModel> GetMissingEntries(this UnitOfWork unit,
                                                            int year, int month, ModelFactory factory)
        {
            List<MissingEntriesModel> missingEntriesList = new List<MissingEntriesModel>();

            int days = DateTime.DaysInMonth(year, month);
            DateTime currentDate = new DateTime(year, month, days);

            var employees = unit.Employees.Get().Where(x => x.BeginDate <= currentDate
                                                            && (x.EndDate == null
                                                            || x.EndDate > currentDate))
                                                            .ToList();

            foreach (var employee in employees)
            {
                List<int> workingDays = DateTimeHelper.ListOfWorkingDays(year, month, 1).ToList();
                var employeeDays = employee.Days.Where(y => y.Date.Month == month && y.Date.Year == year)
                                                    .Select(x => x.Date.Day).ToList();

                var missing = workingDays.Except(employeeDays).ToList();

                if (missing.Count() > 0)
                {
                    EmployeeModel empl = new EmployeeModel()
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Email
                    };

                    MissingEntriesModel emp = new MissingEntriesModel()
                    {
                        Employee = empl,
                        MissingDaysCount = Math.Abs(unit.Days.Get().Where(x => x.Employee.Id == employee.Id
                                                    && x.Date.Year == year && x.Date.Month == month)
                                                .Count() - workingDays.Count()),
                        MissingDays = missing,
                        MailBody = "You are missing an entry"

                    };

                    missingEntriesList.Add(emp);
                }

            }

            return missingEntriesList;
        }

        public static List<Inner> DailyReportForMonth(this UnitOfWork TimeUnit, int year, int month)
        {
            var projects = TimeUnit.Tasks.Get().OrderBy(x => x.Day.Date)
            .Where(t => t.Day.Date.Month == month && t.Day.Date.Year == year)
            .Select(d => new {
                Date = d.Day.Date,
                Hours = d.Hours,
                Name = d.Project.Name
            })
            .GroupBy(t => new { t.Name, t.Date })
            .Select(p => new Outer
            {
                Name = p.Key.Name,
                Date = p.Key.Date,
                Sum = p.Sum(w => w.Hours)
            })
            .GroupBy(p => p.Name)
            .ToList();

            List<Inner> All = new List<Inner>();
            foreach (var project in projects)
            {
                Inner inner = new Inner();
                inner.Name = project.Key;
                var outer = project.Select(n => new Outer { Date = n.Date, Name = n.Name, Sum = n.Sum }).ToList();
                inner.Outer = outer;

                All.Add(inner);
            }

            return All;

        }

        public static PersonalReport GetPersonalReport(this UnitOfWork unit, int employeeId, int year, int month, ModelFactory factory)
        {
            var emp = (EmployeeModel)unit.Employees.Get().Where(x => x.Id == employeeId).ToList()
                                                          .Select(x => factory.Create(x, false)).FirstOrDefault();

            if (emp == null)
                return null;
            
            var days = unit.Days.Get().Where(x => x.Employee.Id == employeeId && x.Date.Year == year && x.Date.Month == month)
                                      .Select(x => new PersonalReportsDay()
                                      {
                                          Date = x.Date,
                                          Type = x.Type.ToString(),
                                          Hours = x.Hours,
                                          Comment = x.Comment,
                                          OvertimeHours = x.Hours - 8,
                                          Tasks = x.Tasks.Select(y => new PersonalReportTask()
                                          {
                                              Hours = y.Hours,
                                              Description = y.Description,
                                              Project = y.Project.Name

                                          }).ToList()
                                      }).OrderBy(y => y.Date).ToList();

            int workingDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                   && x.Date.Year == year
                                                   && x.Date.Month == month
                                                   && (x.Type == DayType.WorkingDay)).Count();

            int vacationDays = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                   && x.Date.Year == year
                                                   && x.Date.Month == month
                                                   && (x.Type == DayType.Vacation)).Count();

            int publicHoliday = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                    && x.Date.Year == year
                                                    && x.Date.Month == month
                                                    && (x.Type == DayType.PublicHoliday)).Count();
            
            int religiousDay= unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                    && x.Date.Year == year
                                                    && x.Date.Month == month
                                                    && (x.Type == DayType.ReligiousDay)).Count();

            int sickLeave= unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                    && x.Date.Year == year
                                                    && x.Date.Month == month
                                                    && (x.Type == DayType.SickLeave)).Count();

            int businessAbsence= unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                    && x.Date.Year == year
                                                    && x.Date.Month == month
                                                    && (x.Type == DayType.BusinessAbsence)).Count();
            
            int otherAbsence = unit.Days.Get().Where(x => x.Employee.Id == employeeId
                                                   && x.Date.Year == year
                                                   && x.Date.Month == month
                                                   && (x.Type == DayType.OtherAbsence)).Count();

            List<int> listDates = null;
            var date = new DateTime(year, month, 1);

            if(emp.BeginDate.Year == date.Year 
                && emp.BeginDate.Month==date.Month 
                && emp.BeginDate.Day < DateTime.DaysInMonth(year, month))
            {
                listDates = DateTimeHelper.ListOfWorkingDays(year, month, emp.BeginDate.Day).ToList();
            }
            else
            {
                listDates = DateTimeHelper.ListOfWorkingDays(year, month).ToList();
            }
            /*
            var distinct = unit.Days.Get().Where(x => x.Employee.Id == employeeId && x.Date.Year == year && x.Date.Month == month)
                          .Select(x => new
                          {
                              Name = x.Tasks.GroupBy(dp => dp.Project.Name)
                          });
                          */
                        var DistinctProjects = unit.Tasks.Get().GroupBy(dp => dp.Project.Name).ToList().Select(y => new PersonalReportDistinct()
                        {
                            Name = y.Key,
                            TotalHours = 0
                        }).ToList();
           /* Dictionary<string, int> groupedCustomerList = unit.Tasks.Get().GroupBy(u => u.Project.Name)
                                      .Select(grp => new { name = grp.Key })
                                      .ToDictionary(grp => grp, grp => 0);
                                      */
            //Dictionary<string, int> tasks = new Dictionary<string, int>();

            List<PersonalReportDistinct> projectsList = new List<PersonalReportDistinct>();
            PersonalReportDistinct projects = new PersonalReportDistinct();

            foreach(var project in DistinctProjects)
            {
                foreach(var day in days)
                {
                    foreach(var task in day.Tasks)
                    {
                        if(project.Name == task.Project)
                        {
                            project.TotalHours += task.Hours;
                        }
                    }
                }
            }

            var report = new PersonalReport()
            {
                DistinctProjects = DistinctProjects,
                Employee = emp,
                WorkingDays = workingDays,
                VacationDays = vacationDays,
                BussinessAbsenceDays = businessAbsence,
                PublicHolidayDays = publicHoliday,
                SickLeaveDays = sickLeave,
                ReligiousDays = religiousDay,
                OtherAbsenceDays = otherAbsence,
                PercentageOfWorkingDays = Math.Round(100 * (double)workingDays / listDates.Count(), 2),
                MissingEntries = (emp.BeginDate.Date > date.Date) ? 0 : listDates.Except(days.Select(x => x.Date.Day)).Count(),
                OvertimeHours = days.Sum(x => x.OvertimeHours),
                Days = days,
                NonWorkingDaysTotal = days.Count() - (workingDays + listDates.Except(days.Select(x => x.Date.Day)).Count())
            };

            return report;
        }

        public static TeamReport GetTeamReport(this UnitOfWork unit, string teamId, int year, int month, ModelFactory factory)
        {
            var employees = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).ToList();
            List<TeamMemberModel> reports = new List<TeamMemberModel>();
            decimal? hours = 0;

            var totalTeamHours = unit.Engagements.Get().Where(x => x.Team.Id == teamId)
                .Select(x => x.Employee)
                .SelectMany(x => x.Days)
                .Where(t => t.Date.Month == month && t.Date.Year == year)
                .Sum(p => p.Hours);

            DayStatisticModel statistika = new DayStatisticModel()
            {
                BusinessAbscenceDays = 0,
                MissingEntries = 0,
                OtherAbscenceDays = 0,
                OvertimeHours = 0,
                PercentageOfWorkingDays = 0,
                PublicHolidays = 0,
                ReligiousDays = 0,
                SickLeaveDays = 0,
                VacationDays = 0,
                WorkingDays = 0
            };
            foreach (var employee in employees)
            {

                var days = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days)
                    .Where(x => x.Employee.Id == employee.Id
                                                    && x.Date.Year == year
                                                    && x.Date.Month == month);
                List<int> listDates = null;
                var date = new DateTime(year, month, 1);

                if (employee.BeginDate.Year == date.Year && employee.BeginDate.Month == date.Month
                    && employee.BeginDate.Day < DateTime.DaysInMonth(year, month))
                {
                    listDates = DateTimeHelper.ListOfWorkingDays(year, month, employee.BeginDate.Day).ToList();
                }
                else
                {
                    listDates = DateTimeHelper.ListOfWorkingDays(year, month).ToList();
                }
                int workingDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days)
                    .Where(x => x.Employee.Id == employee.Id
                                                    && x.Date.Year == year
                                                    && x.Date.Month == month
                                                    && x.Type.ToString() == "WorkingDay").Count();
                statistika.WorkingDays += workingDays;


                decimal? overtime = 0;
                foreach (var day in days)
                {
                    if (day.Hours > 8)
                    {
                        overtime += day.Hours - 8;

                    };
                };
                statistika.OvertimeHours += overtime;

                int vacationDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Year == year
                                                         && x.Date.Month == month
                                                         && x.Type.ToString() == "Vacation").Count();
                statistika.VacationDays += vacationDays;

                int businessAbscenceDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Year == year
                                                         && x.Date.Month == month
                                                         && x.Type.ToString() == "BusinessAbsence").Count();

                statistika.BusinessAbscenceDays += businessAbscenceDays;
                int publicHolidays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Year == year
                                                         && x.Date.Month == month
                                                         && x.Type.ToString() == "PublicHoliday").Count();
                statistika.PublicHolidays += publicHolidays;

                int sickLeaveDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Year == year
                                                         && x.Date.Month == month
                                                         && x.Type.ToString() == "SickLeave").Count();
                statistika.SickLeaveDays += sickLeaveDays;

                int religiousDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Year == year
                                                         && x.Date.Month == month
                                                         && x.Type.ToString() == "ReligiousDay").Count();

                statistika.ReligiousDays += religiousDays;
                int otherAbsenceDays = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                         && x.Date.Year == year
                                                         && x.Date.Month == month
                                                         && x.Type.ToString() == "OtherAbsence").Count();
                statistika.OtherAbscenceDays += otherAbsenceDays;

                decimal? totalhoursss = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(x => x.Employee).SelectMany(y => y.Days).Where(x => x.Employee.Id == employee.Id
                                                        && x.Date.Year == year
                                                        && x.Date.Month == month 
                                                        && x.Type.ToString() == "WorkingDay")
                                                        .Select(x => x.Hours).DefaultIfEmpty(0).Sum();
            
                var missingEntries = (employee.BeginDate.Date > date.Date) ? 0 : listDates.Except(days.Select(x => x.Date.Day)).Count();
                statistika.MissingEntries += missingEntries;
                TeamMemberModel employeeModel = new TeamMemberModel()
                {
                    Employee = new BaseModel()
                    {
                        Id = employee.Id,
                        Name = employee.FirstName + ' ' + employee.LastName
                    },
                    Days = new DayStatisticModel()
                    {
                        WorkingDays = workingDays,
                        VacationDays = vacationDays,
                        BusinessAbscenceDays = businessAbscenceDays,
                        PublicHolidays = publicHolidays,
                        SickLeaveDays = sickLeaveDays,
                        ReligiousDays = religiousDays,
                        OtherAbscenceDays = otherAbsenceDays,
                        OvertimeHours = overtime,
                        PercentageOfWorkingDays = Math.Round(100 * (double)workingDays / listDates.Count(), 2),
                        MissingEntries = missingEntries,

                    },
                    TotalHours = totalhoursss
                };

                reports.Add(employeeModel);
                hours += employeeModel.TotalHours;
            }
            //foreach(var employee in reports)
            //{
            //    hours += employee.TotalHours;
            //}
            int numberOfEmployees = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Count();
            var fullworkingdays = numberOfEmployees * DateTimeHelper.ListOfWorkingDays(year, month).Count();
            statistika.PercentageOfWorkingDays = Math.Round(100 * (double)statistika.WorkingDays / fullworkingdays);


            var report = new TeamReport()
            {
                Id = teamId,
                Name = unit.Teams.Get(teamId).Name,
                NumberOfEmployees = numberOfEmployees,
                NumberOfProjects = unit.Teams.Get(teamId).Projects.Count(),
                Dayss = statistika,
                Reports = reports,
                TotalHours = totalTeamHours,
                Year = year,
                Month = month
                //Members = unit.Engagements.Get().Where(x => x.Team.Id == teamId).Select(y=> GetPersonalReport(unit,y.Employee.Id, year,month,factory)).ToList()
                //Ok(TimeUnit.GetPersonalReport(employeeId, year, month, TimeFactory));
            };
            return report;
        }

        public static CompanyReport GetCompanyReport(this UnitOfWork unit, int year, int month, ModelFactory factory)
        {

            var AllTeams = unit.Teams.Get().ToList();
            List<CompanyReportTeams> Teams = new List<CompanyReportTeams>();
            List<CompanyReportProjects> Projects = new List<CompanyReportProjects>();


            var employeesWithMostOvertime = unit.Days.Get()
                .Where(d => d.Hours > 8 && d.Date.Month == month && d.Date.Year == year)
                .GroupBy(d => new { d.Employee, d.Hours } )
                .Select(d => new OvertimeEmployees{ Name = d.Key.Employee.FirstName + " " + d.Key.Employee.LastName, SumHours = d.Sum(w => w.Hours - 8) }).OrderByDescending(t => t.SumHours)
                .ToList();

            var totalForProjects = unit.Tasks.Get()
                .Where(p => p.Day.Date.Year == year && p.Day.Date.Month == month)
                .GroupBy(d => new { d.Project.Name})
                .Select(t => new TotalForProjects { Name = t.Key.Name, OvertimeHours = t.Sum(w => w.Hours) }).ToList();
       
            foreach (var team in AllTeams)
            {
                decimal? overtimehours = 0;
                var employees = unit.Engagements.Get().Where(x => x.Team.Id == team.Id).Select(x => x.Employee).ToList();
                foreach (var employee in employees)
                {
                    foreach (var day in employee.Days)
                    {
                        if (day.Hours > 8 && day.Date.Month == month && day.Date.Year==year)
                            overtimehours += day.Hours - 8;
                    }
                }

                CompanyReportTeams teamToAdd = new CompanyReportTeams()
                {
                    TeamName = team.Name,
                    OvertimeHours = overtimehours
                };
                Teams.Add(teamToAdd);
            }

            int days = DateTime.DaysInMonth(year, month);
            DateTime currentDate = new DateTime(year, month, days);

            //taking into consideration employee/project who quit/ended in the given month/year
            var numEmployees = unit.Employees.Get().Where(x => x.BeginDate <= currentDate
                                                            && (x.EndDate == null
                                                            || x.EndDate > currentDate
                                                            || x.EndDate.Value.Month == month))
                                                   .Count();

            var numProjects = unit.Projects.Get().Where(x => x.StartDate <= currentDate
                                                            && (x.EndDate == null
                                                            || x.EndDate > currentDate
                                                            || x.EndDate.Value.Month == month))
                                                   .Count();

            //total working hours for every employee
            var totalHours = unit.Days.Get().Where(x => x.Type == DayType.WorkingDay
                                               && x.Date.Year == year && x.Date.Month == month)
                                       .Select(x => (int?)x.Hours)
                                       .Sum() ?? 0;

            int maxPossibleTotalHours = noDaysInMonth(year, month) * 8 * numEmployees;

            //pm utilization
            int pmCount = unit.Employees.Get().Where(x => x.Roles.Id == "MGR")
                                          .Where(x => x.BeginDate <= currentDate
                                                            && (x.EndDate == null
                                                            || x.EndDate > currentDate
                                                            || x.EndDate.Value.Month == month))
                                          .Count();

            var pmWorkingDays = unit.Days.Get().Where(x => x.Type == DayType.WorkingDay
                                                            && x.Date.Month == month
                                                            && x.Date.Year == year)
                                            .Where(x => x.Employee.Roles.Id == "MGR"
                                                            && x.Employee.BeginDate <= currentDate
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > currentDate
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal? pmUtil = 0;
            if ((noDaysInMonth(year, month) * pmCount) != 0)
            {
                pmUtil = Math.Round((decimal)(pmWorkingDays / (decimal)(noDaysInMonth(year, month) * pmCount)) * 100, 2);
            }


            //qa utilization
            int qaCount = unit.Employees.Get().Where(x => x.Roles.Id == "QAE")
                              .Where(x => x.BeginDate <= currentDate
                                                && (x.EndDate == null
                                                || x.EndDate > currentDate
                                                || x.EndDate.Value.Month == month))
                              .Count();

            var qaWorkingDays = unit.Days.Get().Where(x => x.Type == DayType.WorkingDay
                                                            && x.Date.Month == month
                                                            && x.Date.Year == year)
                                            .Where(x => x.Employee.Roles.Id == "QAE"
                                                            && x.Employee.BeginDate <= currentDate
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > currentDate
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal qaUtil = 0;
            if (noDaysInMonth(year, month) * qaCount != 0)
            {
                qaUtil = Math.Round((decimal)(qaWorkingDays / (decimal)(noDaysInMonth(year, month) * qaCount)) * 100, 2);
            }


            //dev utilization
            int devCount = unit.Employees.Get().Where(x => x.Roles.Id == "DEV")
                              .Where(x => x.BeginDate <= currentDate
                                                && (x.EndDate == null
                                                || x.EndDate > currentDate
                                                || x.EndDate.Value.Month == month))
                              .Count();

            var devWorkingDays = unit.Days.Get().Where(x => x.Type == DayType.WorkingDay
                                                            && x.Date.Month == month
                                                            && x.Date.Year == year)
                                            .Where(x => x.Employee.Roles.Id == "DEV"
                                                            && x.Employee.BeginDate <= currentDate
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > currentDate
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal devUtil = 0;
            if (noDaysInMonth(year, month) * devCount != 0)
            {
                devUtil = Math.Round((decimal)(devWorkingDays / (decimal)(noDaysInMonth(year, month) * devCount)) * 100, 2);
            }


            //uiux utilization
            int uiuxCount = unit.Employees.Get().Where(x => x.Roles.Id == "UIX")
                                          .Where(x => x.BeginDate <= currentDate
                                                            && (x.EndDate == null
                                                            || x.EndDate > currentDate
                                                            || x.EndDate.Value.Month == month))
                                          .Count();

            var uiuxWorkingDays = unit.Days.Get().Where(x => x.Type == DayType.WorkingDay
                                                            && x.Date.Month == month
                                                            && x.Date.Year == year)
                                            .Where(x => x.Employee.Roles.Id == "UIX"
                                                            && x.Employee.BeginDate <= currentDate
                                                            && (x.Employee.EndDate == null
                                                                    || x.Employee.EndDate > currentDate
                                                                    || x.Employee.EndDate.Value.Month == month))
                                            .Count();

            decimal uiuxUtil = 0;
            if (noDaysInMonth(year, month) * uiuxCount != 0)
            {
                uiuxUtil = Math.Round((decimal)(uiuxWorkingDays / (decimal)(noDaysInMonth(year, month) * uiuxCount)) * 100, 2);
            }


            //var overtimeTeams = unit.Days.Get().Where(x => x.Date.Year == year && x.Date.Month == month)
            //                          .Select(x => new CompanyReportTeams()
            //                          {
            //                              TeamName = x.Employee.Engagements.Select(y => y.Team.Name).FirstOrDefault(),
            //                              //x.Tasks.Select(y => y.Project.Team.Name).FirstOrDefault(),
            //                              OvertimeHours = 0

            //                          })
            //                          .OrderBy(y => y.TeamName)
            //                          .ToList();

            var companyReport = new CompanyReport
            {
                OvertimeEmployees = employeesWithMostOvertime,
                NumEmployees = numEmployees,
                NumProjects = numProjects,
                TotalHours = totalHours,
                MaxPossibleTotalHours = maxPossibleTotalHours,

                PMUtilization = pmUtil.Value,
                PMCount = pmCount,

                DEVUtilization = devUtil,
                DEVCount = devCount,

                QAUtilization = qaUtil,
                QACount = qaCount,

                UIUXUtilization = uiuxUtil,
                UIUXCount = uiuxCount,

                OvertimeHoursTeams = Teams,
                TotalForProjects = totalForProjects
                //RevenueProjects = revenueProjects

            };

            return companyReport;
        }

        public static int noDaysInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            int daysInMonth = 0;

            for (int i = 1; i <= days; i++)
            {
                DateTime day = new DateTime(year, month, i);
                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysInMonth++;
                }
            }

            return daysInMonth;
        }

        public static List<ProjectInvoiceModel> ListInvoices(this UnitOfWork unit, int year, int month)
        {
            int incNumber = 0;
            string nyNumber = "s" + incNumber.ToString("00");
            var listInvoices = unit.Projects.Get().SelectMany(x => x.Tasks)
                                                  .Where(x => x.Day.Date.Month == month && x.Day.Date.Year == year)
                                                  .GroupBy(y => y.Project)
                                                  .Select(w => new ProjectInvoiceModel()
                                                  {
                                                      ProjectName = w.Key.Name,
                                                      StartDate = w.Key.StartDate,
                                                      EndDate = w.Key.EndDate,
                                                      TeamName = w.Key.Team.Name,
                                                      CustomerName = w.Key.Customer.Name,
                                                      CustomerAddress = w.Key.Customer.Address.Road + ", " + w.Key.Customer.Address.City + "-" + w.Key.Customer.Address.ZipCode,
                                                      CustomerContact = w.Key.Customer.Contact,
                                                      CustomerEmail = w.Key.Customer.Email,
                                                      CustomerPhone = w.Key.Customer.Phone,
                                                      Amount = w.Key.Amount,
                                                      MailBody = "Here is an invoice for your company",
                                                      InvoiceNumber = year.ToString() + "/" + month.ToString() + "-" + w.Key.Name.Substring(0, 3) + nyNumber,
                                                      InvoiceDate = DateTime.Now,
                                                      Roles = w.Key.Team.Engagements
                                                                        .Where(t => t.Team.Id == w.Key.TeamId)
                                                                        .GroupBy(r => r.Role)
                                                                        .Select(x => new RoleInvoiceModel()
                                                                        {
                                                                            Description = x.Key.Name,
                                                                            Quanity = x.Key.Engagements
                                                                                           .Where(t => t.Team.Id == w.Key.TeamId)
                                                                                           .Select(em => em.Employee)
                                                                                           .SelectMany(d => d.Days)
                                                                                           .Where(g => g.Date.Month == month && g.Date.Year == year && g.Type == DayType.WorkingDay)
                                                                                           .SelectMany(a => a.Tasks)
                                                                                           .Where(p => p.Project.Id == w.Key.Id)
                                                                                           .Select(h => h.Hours)
                                                                                           .DefaultIfEmpty(0)
                                                                                           .Sum(),
                                                                            Unit = "Hours",
                                                                            UnitPrice = x.Key.Hrate
                                                                        })
                                                                        .ToList(),
                                                      
                                                  })
                                                  .ToList();


            return listInvoices;
        }



    }
}