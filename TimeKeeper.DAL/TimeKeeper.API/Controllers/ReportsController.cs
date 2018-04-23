using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeKeeper.API.Controllers.Helpers.ReportsHelpers;

namespace TimeKeeper.API.Controllers
{

    public class ReportsController : BaseController
    {
        [Route("api/reports/personalreport")]
        public IHttpActionResult GetPersonalReportForEmployee(int employeeId, int year, int month)
        {
            if (TimeUnit.Employees.Get(employeeId) == null)
                return NotFound();
            return Ok(TimeUnit.GetPersonalReport(employeeId, year, month, TimeFactory));
        }

        [Route("api/reports/monthlyreport")]
        public IHttpActionResult GetMonthlyReport(int year = 0, int month = 0)
        {
            if (year == 0) year = DateTime.Today.Year;
            if (month == 0) month = DateTime.Today.Month;
            return Ok(TimeReports.MonthlyReport(year, month));
        }

        [Route("api/reports/annualreport")]
        public IHttpActionResult GetAnnualReport(int year = 0)
        {
            if (year == 0) year = DateTime.Today.Year;
            return Ok(new
            {
                year,
                list = TimeReports.AnnualReport(year)
            });
        }

        [Route("api/reports/projecthistory")]
        public IHttpActionResult GetProjectHistory(int projectId)
        {
            return Ok(TimeReports.ProjectHistory(projectId));
        }

        [Route("api/reports/TeamReport")]
        public IHttpActionResult GetTeamReport(string teamId, int year, int month)
        {
            if (TimeUnit.Teams.Get(teamId) == null) return NotFound();
            return Ok(TimeUnit.GetTeamReport(teamId, year, month, TimeFactory));
        }

        [Route("api/reports/company/{year}/{month}")]
        public IHttpActionResult GetCompanyReport(int year, int month)
        {
            return Ok(TimeUnit.GetCompanyReport(year, month, TimeFactory));
        }

        [Route("api/reports/monthlyByDays/{year}/{month}")]
        public IHttpActionResult getMonthlyByDays(int year, int month)
        {
            return Ok(TimeUnit.DailyReportForMonth(year, month));
        }
        
        [Route("api/reports/listInvoices/{year}/{month}")]
        public IHttpActionResult getInvoices(int year, int month)
        {
            return Ok(TimeUnit.ListInvoices(year, month));
        }
        


    }
}
