using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeKeeper.API.Controllers.Helpers;
using TimeKeeper.DAL;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Models
{
    public class ModelFactory
    {
        public TeamModel Create(Team t, bool details = false)
        {
            if(details == true)
            { 
                return new TeamDetailsModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Image = t.Image,
                    Members = t.Engagements.Where(x => x.Employee != null).Select(e => Create(e)).ToList(),
                    Projects = t.Projects.Select(p => Create(p)).ToList()
                };
            }
            else
            {
                return new TeamModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Members = t.Engagements.Where(x => x.Employee != null).Select(e => Create(e)).ToList(),
                    Description = t.Description
                };
            }
        }

        public RoleModel Create(Role r, bool details = false)
        {
            if(details == true) { 
                return new RoleDetailsModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Type = r.Type.ToString(),
                    Hrate = r.Hrate,
                    Mrate = r.Mrate,
                    Members = r.Engagements.Select(e => Create(e)).ToList()
                };
            }
            else
            {
                return new RoleModel()
                {
                    Id = r.Id,
                    Name = r.Name
                };
            }
        }

        public EngagementModel Create(Engagement e, bool details = false)
        {
            if (details == true)
            {
                return new EngagementDetailsModel()
                {
                    Id = e.Id,
                    Team = new RoleModel { Id=e.Team.Id, Name=e.Team.Name},
                    Role = new RoleModel { Id=e.Role.Id, Name=e.Role.Name},
                    Employee = new BaseModel { Id=e.Employee.Id, Name=e.Employee.FirstName+" "+ e.Employee.LastName},
                    Hours = e.Hours
                };
            }
            else
            {
                return new EngagementModel()
                {
                    Id = e.Id,
                    Team = new RoleModel { Id = e.Team.Id, Name = e.Team.Name },
                    Role = new RoleModel { Id = e.Role.Id, Name = e.Role.Name },
                    Employee = new BaseModel { Id = e.Employee.Id, Name = e.Employee.FirstName + " " + e.Employee.LastName },
                    Hours = e.Hours
                };
            }
        }

        public ProjectModel Create(Project p)
        {
            //if (details == true)
            //{
            //    return new ProjectDetailsModel()
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Monogram = p.Monogram,
            //        Description = p.Description,
            //        StartDate = p.StartDate,
            //        EndDate = p.EndDate,
           // Status = p.Status.ToString(),
             //       Pricing = p.Status.ToString(),
            //        Amount = p.Amount,
            //        Customer = (p.Customer == null) ? 0 : p.Customer.Id,
            //        Team = (p.Team == null) ? "none" : p.Team.Id

            //    };
            //}
            //else
            //{
                return new ProjectModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Monogram = p.Monogram,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status.ToString(),
                    Pricing = p.Pricing.ToString(),
                    Amount = p.Amount,
                    Customer = (p.Customer != null) ? p.Customer.Name:"/",
                    CustomerId = (p.Customer !=null)? p.CustomerId: 0,
                    Team = (p.Team != null)? p.Team.Name : "/",
                    TeamId =(p.Team!= null)?p.TeamId : "/"

                };

        }
        
        public TaskModel Create(Task t)
        {
           
           return new TaskModel()
                {
                    Id = t.Id,
                    Description = t.Description,
                    Hours = t.Hours,
                    Deleted = t.Deleted,
                    Project = new BaseModel { Id = t.Project.Id, Name = t.Project.Name }
                };
        }
        


        public CustomerModel Create(Customer c, bool details = false)
        {
            if (details == true)
            {
                return new CustomerDetailsModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = "data:image/png;base64, " + c.ConvertToBase64(),
                    Monogram = c.Monogram,
                    Contact = c.Contact,
                    Email = c.Email,
                    Phone = c.Phone,
                    Status = c.Status.ToString(),
                    Address_City = c.Address.City,
                    Address_ZipCode = c.Address.ZipCode,
                    Address_Road = c.Address.Road,
                    Projects = c.Projects.Select(p => Create(p)).ToList()
                };
            }
            else
            {
                return new CustomerModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = "data:image/png;base64, " + c.ConvertToBase64(),
                    Monogram = c.Monogram,
                    Contact = c.Contact,
                    Email = c.Email,
                    Phone = c.Phone,
                    Status = c.Status.ToString(),
                    Address_City = c.Address.City,
                    Address_ZipCode = c.Address.ZipCode,
                    Address_Road = c.Address.Road
                };
            }
        }

        public EmployeeModel Create(Employee e, bool details = false)
        {
            if (details == true)
            {
                return new EmployeeDetailsModel()
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Image = "data:image/png;base64, " + e.ConvertToBase64(),
                    Email = e.Email,
                    Phone = e.Phone,
                    BirthDate = e.BirthDate,
                    BeginDate = e.BeginDate,
                    EndDate = e.EndDate,
                    Salary = e.Salary,
                    Status = e.Status.ToString(),
                    Roles = Create(e.Roles),
                    //Days = e.Days.Select(p => Create(p)).ToList(),
                    Engagements = e.Engagements.Select(p => Create(p)).ToList()
                };
            }
            else
            {
                return new EmployeeModel()
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Image = "data:image/png;base64, " + e.ConvertToBase64(),
                    Email = e.Email,
                    Phone = e.Phone,
                    BirthDate = e.BirthDate,
                    BeginDate = e.BeginDate,
                    EndDate = e.EndDate,
                    Salary = e.Salary,
                    Status = e.Status.ToString(),
                    Roles = Create(e.Roles)
                };
            }
        }
        public UserModel CreateUser(Employee emp, string provider)
        {
                return new UserModel
                {
                    Id = emp.Id,
                    Name = emp.FirstName + " " + emp.LastName,
                    Role = emp.Roles.Name,
                    Teams = emp.Engagements.Where(x=>x.Role.Id.Contains("TL")).Select(x=> x.Team).Select(x=>Create(x,emp)).ToList(),
                    //Token = id_token
                    Provider=provider,
                    Image= "data:image/png;base64, " + emp.ConvertToBase64()
                };
           
        }

        public UserModelTeam Create (Team t, Employee emp)
        {
            return new UserModelTeam()
            {
                Id = t.Id,
                Name = t.Name,
                Role = emp.Engagements.Where(x => x.Team.Id == t.Id).Select(x => x.Role).Select(x => Create(x)).FirstOrDefault()
            };
        }
        //public DayModel Create(Day d)
        //{   
        //        return new DayModel()
        //        {
        //            Date = d.Date,
        //            Type = d.Type.ToString(),
        //            Hours = d.Hours,
        //            Comment = d.Comment,
        //            Employee = d.Employee.FirstName + " " + d.Employee.LastName
        //        };
        //    }
        //}
    }
}