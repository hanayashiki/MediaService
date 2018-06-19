using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.DBManager
{
    public class MongoDatabaseContext
    {
        readonly MongoDbConfig config;
        readonly MongoClient client;
        protected readonly IMongoDatabase database;

        public MongoDatabaseContext(MongoDbConfig config)
        {
            this.config = config;
            (string username, string password) = GetAuth(config.AuthenticationFile);
            string mongoDbConnection = string.Format(config.Uri, $"{username}:{password}@", config.DatabaseName);
            Console.WriteLine("linking to mongodb by: " + mongoDbConnection);
            this.client = new MongoClient();
            this.database = client.GetDatabase(config.DatabaseName);
        }
        private (string, string) GetAuth(string authenticationFile)
        {

            String jsonStr = File.ReadAllText(authenticationFile);
            Console.WriteLine(jsonStr);
            JObject auth = JObject.Parse(jsonStr);
            return (auth["Username"].Value<string>(), auth["Password"].Value<string>());
        }

    }
}
