using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TimeKeeper.MailDB
{
    public class BasicService
    {

        private static readonly BasicService instance = new BasicService();

        private MongoClient client;
        private IMongoDatabase dBase;

        public MongoClient Client { get { return client; } }
        public IMongoDatabase DBase { get { return dBase; } }

        private BasicService()
        {
            client = new MongoClient();
            dBase = client.GetDatabase("TimeKeeperStorage");
        }

        public static BasicService Instance {
            get { return instance; }
        }
    }
}
