using System;
using System.Data.Entity;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repositories;

namespace TimeKeeper.DAL
{
    internal class TimeKeeperDbInitializer<T> : DropCreateDatabaseAlways<TimeKeeperDbContext>
    {
        public override void InitializeDatabase(TimeKeeperDbContext context)
        {
            try
            {
                // ensure that old database instance can be dropped
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
                        $"ALTER DATABASE {context.Database.Connection.Database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            }
            catch
            {
                // database does not exists - no problem ;o)
            }
            finally
            {
                base.InitializeDatabase(context);

                using (UnitOfWork unit = new UnitOfWork())
                {
                    addRoles(unit);
                    addTeams(unit);
                    addEmployees(unit);
                    addCustomers(unit);
                    addDays(unit);
                    addProjects(unit);
                    addTasks(unit);
                    addEngagements(unit);
                }
            }
        }

        void addEngagements(UnitOfWork unit)
        {
            unit.Engagements.Insert(new Engagement()
            {
                Hours = 20,
                Employee = unit.Employees.Get(1),
                Role = unit.Roles.Get("TL"),
                Team = unit.Teams.Get("A")
            });

            unit.Engagements.Insert(new Engagement()
            {
                Hours = 40,
                Employee = unit.Employees.Get(1),
                Role = unit.Roles.Get("TL"),
                Team = unit.Teams.Get("A")
            });

            unit.Save();
        }

        void addRoles(UnitOfWork unit)
        {
            unit.Roles.Insert(new Role()
            {
                Id = "SD",
                Name = "Software Developer",
                Type = RoleType.RoleInTeam,
                Hrate = 30,
                Mrate = 4500
            });
            unit.Roles.Insert(new Role()
            {
                Id = "TL",
                Name = "Team Lead",
                Type = RoleType.RoleInTeam,
                Hrate = 40,
                Mrate = 6000
            });
            unit.Save();
        }

        void addTeams(UnitOfWork unit)
        {
            unit.Teams.Insert(new Team()
            {
                Name = "Alpha",
                Id = "A",
                Image = "A",
                Description = "Alpha Team"
            });
            unit.Teams.Insert(new Team()
            {
                Name = "Bravo",
                Id = "B",
                Image = "B",
                Description = "Bravo Team"
            });

            unit.Save();
        }

        void addEmployees(UnitOfWork unit)
        {
            unit.Employees.Insert(new Employee() {
                FirstName = "Edo",
                LastName = "Pleh",
                Email = "johndoe@gmail.com",
                BirthDate = DateTime.Now.AddYears(-32),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-1),
                Salary = 2200,
                Roles = unit.Roles.Get("TL")
            });
            unit.Employees.Insert(new Employee()
            {
                FirstName = "Edo",
                LastName = "Pleh",
                Email = "edopleh@gmail.com",
                BirthDate = DateTime.Now.AddYears(-22),
                Status = EmployeeStatus.Active,
                BeginDate = DateTime.Now.AddYears(-2),
                Salary = 1500,
                Roles = unit.Roles.Get("TL"),

            });
            unit.Save();
        }

        void addCustomers(UnitOfWork unit)
        {
            unit.Customers.Insert(new Customer()
            {
                Name = "Redzo Bekto",
                Status = CustomerStatus.Client,
                Contact = "Redzo Bekto",
                Email = "bekto@preciza.ba",
                Phone = "061 123 456"
            });
            unit.Customers.Insert(new Customer()
            {
                Name = "Boomerang",
                Status = CustomerStatus.Prospect,
                Contact = "Samir Hasagic",
                Email = "boomerang@boomerang.ba",
                Phone = "038 222 222"
            });
            unit.Save();
        }

        void addDays(UnitOfWork unit)
        {
            unit.Days.Insert(new Day()
            {
                Date = DateTime.Now,
                Type = DayType.WorkingDay,
                Hours = 8,
                Employee = unit.Employees.Get(1)
            });

            unit.Days.Insert(new Day()
            {
                Date = DateTime.Now.AddDays(-1),
                Type = DayType.WorkingDay,
                Hours = 8,
                Employee = unit.Employees.Get(1)
            });
            unit.Save();
        }
        void addProjects(UnitOfWork unit)
        {
            unit.Projects.Insert(new Project()
            {
                Name = "NASA Project",
                Description = "Some NASA Project",
                StartDate = DateTime.Now.AddDays(-365),
                Status = ProjectStatus.InProgress,
                Pricing = PricingStatus.FixedPrice,
                Team = unit.Teams.Get("A"),
                Customer = unit.Customers.Get(1)
            });
            unit.Projects.Insert(new Project()
            {
                Name = "IMT Rezervni Dijelovi za Bagere",
                Description = "Some IMT Project",
                StartDate = DateTime.Now.AddDays(-465),
                EndDate = DateTime.Now.AddDays(-110),
                Status = ProjectStatus.Finished,
                Pricing = PricingStatus.HourlyRate,
                Team = unit.Teams.Get("A"),
                Customer = unit.Customers.Get(1)
            });
          
            unit.Save();
        }
        void addTasks(UnitOfWork unit)
        {

            unit.Tasks.Insert(new Entities.Task()
            {
                Description = "Take Protein Pills",
                Hours = 4,
                Day = unit.Days.Get(1),
                Project = unit.Projects.Get(1)
            });

            unit.Tasks.Insert(new Entities.Task()
            {
                Description = "Commencing Countdown",
                Hours = 4,
                Day = unit.Days.Get(1),
                Project = unit.Projects.Get(1)
            });
            unit.Save();

        }


    }
}

//Database.SetInitializer<TimeContext>(new CreateDatabaseIfNotExists<TimeContext>());
//Database.SetInitializer<TimeContext>(new DropCreateDatabaseIfModelChanges<TimeContext>());
//base.Database.ExecuteSqlCommand("USE master; ALTER DATABASE Testera SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
//Database.SetInitializer<TimeContext>(new DropCreateDatabaseAlways<TimeContext>());