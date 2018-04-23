using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TimeKeeper.API.Controllers.Helpers.ReportsHelpers;
using TimeKeeper.API.Models;
using TimeKeeper.API.Reports;
using TimeKeeper.DAL.Entities;
using TimeKeeper.MailDB;
using TimeKeeper.MailDB.Entities;

namespace TimeKeeper.API.Controllers
{
    public class MissingEntriesController : BaseController
    {
        [System.Web.Http.Route("api/missingEntries/{year}/{month}")]
        public IHttpActionResult Get(int year, int month)
        {
            return Ok(TimeUnit.GetMissingEntries(year, month, TimeFactory));

        }

        [System.Web.Http.Route("api/missingEntries")]
        public IHttpActionResult NotifyForMissingEntries([FromBody] List<MissingEntriesModel> employees)
        {
            try
            {
                MailStorageService mailService = new MailStorageService();
                foreach (var e in employees)
                {

                    //var employee = TimeUnit.Employees.Get(x => x.Id == e.Employee.Id).FirstOrDefault();
                    //var mailBody = e.MailBody(employee);
                    var mailBody = e.MailBody;

                    mailService.StoreMails(new MailContent()
                    {
                        ReceiverMailAddress = e.Employee.Email,
                        MailBody = mailBody,
                        MailSubject = "Missing Entries Notification",
                        DateCreated = DateTime.Now
                    });

                }
                return Ok("Successifully sent messages.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

