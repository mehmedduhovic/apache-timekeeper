using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeKeeper.MailDB.Entities;

namespace TimeKeeper.MailDB
{
    public class MailStorageService
    {
        private BasicService CommunicationService;

        public MailStorageService()
        {
            CommunicationService = BasicService.Instance;
        }

        public void StoreMails(MailContent content)
        {
            try{ 
                var collection = CommunicationService.DBase.GetCollection<MailContent>("Mails");
                collection.InsertOne(content);
            }
            catch {
                throw new Exception("Mails have not been saved properly");
            }
        }
    }
}
