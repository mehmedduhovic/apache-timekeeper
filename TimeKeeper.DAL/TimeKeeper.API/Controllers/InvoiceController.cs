using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeKeeper.API.Models;
using TimeKeeper.MailDB;
using TimeKeeper.MailDB.Entities;

namespace TimeKeeper.API.Controllers
{
    public class InvoiceController : BaseController
    {

        [System.Web.Http.Route("api/invoice")]
        public IHttpActionResult Post ([FromBody] ProjectInvoiceModel invoice)
        {
            try
            {
            //    var conString = "mongodb://localhost:27017";
            //    var client = new MongoClient(conString);
                MailStorageService mailService = new MailStorageService();
                
                    //var employee = TimeUnit.Employees.Get(x => x.Id == e.Employee.Id).FirstOrDefault();
                    //var mailBody = e.MailBody(employee);
                    var mailBody = invoice.MailBody;

                    mailService.StoreMails(new MailContent()
                    {
                        ReceiverMailAddress = invoice.CustomerEmail,
                        MailBody = mailBody,
                        MailSubject = "Invoices Notification",
                        DateCreated = DateTime.Now
                    });

                return Ok("Successifully sent messages.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
