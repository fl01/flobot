using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Flobot.ExternalServiceCore.Storage
{
    public abstract class MongoDbStorageBase
    {
        protected IMongoClient MongoClient { get; private set; }

        protected IMongoDatabase MongoDb { get; private set; }

        public MongoDbStorageBase(string connectionString, string dbName)
        {
            MongoClient = new MongoClient(connectionString);
            MongoDb = MongoClient.GetDatabase(dbName);
        }
    }
}
