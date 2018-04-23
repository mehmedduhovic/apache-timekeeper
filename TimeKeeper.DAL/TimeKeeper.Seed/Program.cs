using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.Seed
{
    class Program
    {
        static TimeKeeperDbContext context = new TimeKeeperDbContext();
        static DataTable rawData = new DataTable();

        static Dictionary<int, int> customersDictionary = new Dictionary<int, int>();
        static Dictionary<int, int> projectsDictionary = new Dictionary<int, int>();
        static Dictionary<int, int> employeesDictionary = new Dictionary<int, int>();
        static Dictionary<int, int> calendarDictionary = new Dictionary<int, int>();

        static void Main(string[] args)
        {
            ExcelReader.DataSource = @"C:\Projects\Timer.xls";

            using (context)
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                SeedTeams();
                SeedRoles();
                SeedEmployees();
                SeedEngagement();

                SeedCustomers();
                SeedProjects();

                SeedCalendar();
                SeedDetails();
            }

            Console.WriteLine("\n*** MIGRATION DONE ***");
            Console.ReadKey();
        }

        private static void SeedTeams()
        {
            Console.Write("Teams: ");
            rawData = rawData.Open("Teams");
            foreach (DataRow row in rawData.Rows)
            {
                context.Teams.Add(new Team()
                {
                    Id = (string)row.Read(0, typeof(string)),
                    Name = (string)row.Read(1, typeof(string)),
                    Description = (string)row.Read(2, typeof(string))
                });
            }
            context.SaveChanges();
            Console.WriteLine(context.Teams.Count());
        }

        private static void SeedRoles()
        {
            Console.Write("Roles: ");
            rawData = rawData.Open("Roles");
            foreach (DataRow row in rawData.Rows)
            {
                context.Roles.Add(new Role()
                {
                    Id = (string)row.Read(0, typeof(string)),
                    Name = (string)row.Read(1, typeof(string)),
                    Hrate = (decimal)row.Read(2, typeof(decimal)),
                    Mrate = (decimal)row.Read(3, typeof(decimal)),
                    Type = (RoleType)row.Read(4, typeof(int))
                });
            }
            context.SaveChanges();
            Console.WriteLine(context.Roles.Count());
        }

        private static void SeedEmployees()
        {
            Console.Write("Employees: ");
            rawData = rawData.Open("Employees");
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = (int)row.Read(0, typeof(int));
                Employee employee = new Employee()
                {
                    FirstName = (string)row.Read(1, typeof(string)),
                    LastName = (string)row.Read(2, typeof(string)),
                    Image = (string)row.Read(3, typeof(string)),
                    Email = (string)row.Read(4, typeof(string)),
                    Phone = (string)row.Read(5, typeof(string)),
                    BirthDate = (DateTime)row.Read(6, typeof(DateTime)),
                    BeginDate = (DateTime)row.Read(7, typeof(DateTime)),
                    Status = (EmployeeStatus)row.Read(9, typeof(int)),
                    Roles = context.Roles.Find((string)row.Read(10, typeof(string))),
                    Salary = (decimal)row.Read(11, typeof(decimal))
                };
                if (employee.Status == EmployeeStatus.Leaver)
                    employee.EndDate = (DateTime)row.Read(8, typeof(DateTime));
                context.Employees.Add(employee);
                context.SaveChanges();
                employeesDictionary.Add(oldId, employee.Id);
            }
            Console.WriteLine(context.Employees.Count());
        }

        private static void SeedEngagement()
        {
            Console.Write("Engagement: ");
            rawData = rawData.Open("Engagement");
            foreach (DataRow row in rawData.Rows)
            {
                context.Engagements.Add(new Engagement()
                {
                    Employee = context.Employees.Find(employeesDictionary[(int)row.Read(0, typeof(int))]),
                    Team = context.Teams.Find((string)row.Read(1, typeof(string))),
                    Role = context.Roles.Find((string)row.Read(2, typeof(string))),
                    Hours = (decimal)row.Read(3, typeof(decimal))
                });
            }
            context.SaveChanges();
            Console.WriteLine(context.Engagements.Count());
        }

        private static void SeedCustomers()
        {
            Console.Write("Customers: ");
            rawData = rawData.Open("Customers");
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = (int)row.Read(0, typeof(int));
                Customer customer = new Customer()
                {
                    Name = (string)row.Read(1, typeof(string)),
                    Monogram = (string)row.Read(2, typeof(string)),
                    Contact = (string)row.Read(3, typeof(string)),
                    Email = (string)row.Read(4, typeof(string)),
                    Phone = (string)row.Read(5, typeof(string)),
                    Address =
                    {
                        Road = (string)row.Read(6, typeof(string)),
                        ZipCode=(string)row.Read(7, typeof(string)),
                        City = (string)row.Read(8, typeof(string))
                    },
                    Status = (CustomerStatus)row.Read(9, typeof(int))
                };
                context.Customers.Add(customer);
                context.SaveChanges();
                customersDictionary.Add(oldId, customer.Id);
            }
            Console.WriteLine(context.Customers.Count());
        }

        private static void SeedProjects()
        {
            Console.Write("Projects: ");
            rawData = rawData.Open("Projects");
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = (int)row.Read(0, typeof(int));
                Project project = new Project()
                {
                    Monogram = (string)row.Read(1, typeof(string)),
                    Name = (string)row.Read(2, typeof(string)),
                    Description = (string)row.Read(3, typeof(string)),
                    StartDate = (DateTime)row.Read(4, typeof(DateTime)),
                    EndDate = (DateTime)row.Read(5, typeof(DateTime)),
                    Status = (ProjectStatus)row.Read(6, typeof(int)),
                    Customer = context.Customers.Find(customersDictionary[(int)row.Read(7, typeof(int))]),
                    Team = context.Teams.Find((string)row.Read(8, typeof(string))),
                    Pricing = (PricingStatus)row.Read(9, typeof(int)),
                    Amount = (decimal)row.Read(10, typeof(decimal))
                };
                context.Projects.Add(project);
                context.SaveChanges();
                projectsDictionary.Add(oldId, project.Id);
            }
            Console.WriteLine(context.Projects.Count());
        }

        private static void SeedCalendar()
        {
            Console.Write("Calendar: ");
            rawData = rawData.Open("Calendar");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                int oldId = (int)row.Read(0, typeof(int));
                Day day = new Day()
                {
                    Employee = context.Employees.Find(employeesDictionary[(int)row.Read(1, typeof(int))]),
                    Type = (DayType)row.Read(2, typeof(int)),
                    Date = (DateTime)row.Read(3, typeof(DateTime)),
                    Hours = (decimal)row.Read(4, typeof(decimal))
                };
                context.Days.Add(day);
                context.SaveChanges();
                if (++N % 100 == 0) Console.Write($"{N} ");
                calendarDictionary.Add(oldId, day.Id);
            }
            Console.WriteLine(context.Days.Count());
        }

        private static void SeedDetails()
        {
            Console.Write("Details: ");
            rawData = rawData.Open("Details");
            int N = 0;
            foreach (DataRow row in rawData.Rows)
            {
                context.Tasks.Add(new Task()
                {
                    Description = (string)row.Read(0, typeof(string)),
                    Hours = (decimal)row.Read(1, typeof(decimal)),
                    Project = context.Projects.Find(projectsDictionary[(int)row.Read(2, typeof(int))]),
                    Day = context.Days.Find(calendarDictionary[(int)row.Read(3, typeof(int))]),
                });
                if (++N % 100 == 0) Console.Write($"{N} ");
            }
            context.SaveChanges();
            Console.WriteLine(context.Tasks.Count());
        }
    }
}

