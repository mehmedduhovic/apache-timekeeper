using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.ModelBinding;
using TimeKeeper.DAL.Entities;

namespace TimeKeeper.API.Controllers.Helpers.Validations
{
    public static class ValidateExtensions
    {
        public static List<string> ValidateCustomer(this Customer customer)
        {
            List<string> errorMsg = new List<string>();
            if (customer.Name.Length < 3)
            {
                errorMsg.Add("The contact name of the customer must contain at least 3 characters!");
            }
            if (customer.Contact.Length <= 3)
            {
                errorMsg.Add("The contact name of the customer must contain 3 or more characters!");
            }
            if (!Regex.Match(customer.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Success)
            {
                errorMsg.Add("The company email is in the invalid format.");
            }

            if(customer.Status != CustomerStatus.Prospect && customer.Status != CustomerStatus.Client)
            {
                errorMsg.Add("The Status of the company is invalid.");
            }
            return errorMsg;
        }

        public static List<string> ValidateTeam(this Team team)
        {
            List<string> errorMsg = new List<string>();
            if (team.Name.Length < 3)
            {
                errorMsg.Add("The name of the team must contain at least 3 characters!");
            }

            if (!Regex.Match(team.Name, @"^[a-zA-Z0-9_ ]+$").Success)
            {
                errorMsg.Add("The team name can only contain letters, numbers and underscore.");
            }

            if (!Regex.Match(team.Id, @"^[a-zA-Z]+$").Success)
            {
                errorMsg.Add("The team id is required and it can only cointain letters.");
            }
            return errorMsg;
        }

        public static List<string> ValidateDays(this Day days)
        {
            List<string> errorMsg = new List<string>();
            if (days.Hours > 12)
            {
                errorMsg.Add("The working hours for this particular day are too long!");
            }

            
            if(days.Type != DayType.BusinessAbsence && days.Type != DayType.OtherAbsence && days.Type != DayType.PublicHoliday
                && days.Type != DayType.ReligiousDay && days.Type != DayType.SickLeave && days.Type != DayType.Vacation 
                && days.Type != DayType.WorkingDay)
            {
                errorMsg.Add("The type of the day is invalid!");
            }

            return errorMsg;
        }

        public static List<string> ValidateEmployee (this Employee employee)
        {
            List<string> errorMsg = new List<string>();
         
            if (employee.Status != EmployeeStatus.Active
                && employee.Status != EmployeeStatus.Trial
                && employee.Status != EmployeeStatus.Leaver)
            {
                errorMsg.Add("The employee status must be trial, active or leaver!");
            }          
            if (employee.FirstName.Length < 2)
            {
                errorMsg.Add("The first name of the employee must contain at least 2 characters!");
            }           
            var regexName = new Regex("^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$");

            if (!regexName.IsMatch(employee.FirstName))
            {
                errorMsg.Add("First Name can't contain special characters and numbers.");
            }           
            if (employee.LastName.Length < 3)
            {
                errorMsg.Add("The last name of the employee must contain at least 3 characters!");
            }            
            if (!regexName.IsMatch(employee.LastName))
            {
                errorMsg.Add("Last Name can't contain special characters and numbers.");
            }              
            if (!Regex.Match(employee.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Success)
            {
                errorMsg.Add("The employee email is in the invalid format.");
            }
            if (employee.Salary<410 || employee.Salary>10000)
            {
                errorMsg.Add("Minimum salary is 410 and maximum 10000");
            }           
            return errorMsg;
        }
        public static List<string> ValidateProject(this Project project)
        {
            List<string> errorMsg = new List<string>();
            var regexName = new Regex("^[a-zA-Z0-9 ]*$");

            if (!regexName.IsMatch(project.Name))
            {
                errorMsg.Add("Project name can't contain special characters.");
            }

            if (project.Name.Length < 3)
            {
                errorMsg.Add("The name of the project must contain at least 3 characters!");
            }

            if (project.Status != ProjectStatus.InProgress
                && project.Status != ProjectStatus.OnHold
                && project.Status != ProjectStatus.Finished
                && project.Status != ProjectStatus.Canceled)
            {
                errorMsg.Add("The project status must be InProgress, OnHold, Finished or Canceled!");
            }
            //if (project.Status < 0)
            //{
            //    errorMsg.Add("The project status must be InProgress, OnHold, Finished or Canceled!");

            //}
            if (project.Pricing != PricingStatus.FixedPrice
                && project.Pricing != PricingStatus.HourlyRate
                && project.Pricing != PricingStatus.NotBillable
                && project.Pricing != PricingStatus.PerCapitaRate)
            {
                errorMsg.Add("The project status must be FixedPrice, HourlyRate, NotBillable or PerCapitaRate!");
            }
            return errorMsg;
        }

        public static List<string> ValidateEngagements(this Engagement engagement)
        {
            List<string> errorMsg = new List<string>();
            if (engagement.Hours > 40)
            {
                errorMsg.Add("Engagement hours can not be longer than 40 hours.");
            }

            return errorMsg;

        }

        public static List<string> ValidateRole(this Role role)
        {
            List<string> errorMsg = new List<string>();

            if(role.Type != RoleType.JobTitle && role.Type != RoleType.RoleInApp && role.Type != RoleType.RoleInTeam)
            {
                errorMsg.Add("Roles - Invalid role type entered.");
            }

            if((role.Type == RoleType.RoleInTeam && role.Hrate < 0) || (role.Type == RoleType.RoleInTeam && role.Hrate > 120))
            {
                errorMsg.Add("Roles - Invalid hourly rate entered.");
            }

            if ((role.Type == RoleType.RoleInTeam && role.Mrate < 0) || (role.Type == RoleType.RoleInTeam && role.Mrate > 250000))
            {
                errorMsg.Add("Roles - Invalid monthly rate entered.");
            }

            return errorMsg;
        }

        public static List<string> ValidateTask(this Task task)
        {
            List<string> errorMsg = new List<string>();

            if(task.Hours <= 0)
            {
                errorMsg.Add("Task hours cannot be zero");
            }

            if (task.Hours > 8)
            {
                errorMsg.Add("Task hours cannot be more than 8");
            }

            return errorMsg;
        }
    }
}