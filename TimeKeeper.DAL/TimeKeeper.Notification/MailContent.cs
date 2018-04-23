using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.Notification
{
    public class MailContent
    {
            public ObjectId Id { get; set; }
            public string ReceiverMailAddress { get; set; }
            public string MailSubject { get; set; }
            public string MailBody { get; set; }
            public DateTime DateCreated { get; set; }
        
    }
}
