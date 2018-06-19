using Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.DBManager
{
    public class MongoDbConfig
    {
        public string Uri { get; set; }
        public string DatabaseName { get; set; }
        public string AuthenticationFile { get; set; }


        public static MongoDbConfig GetMongoDbConfig(string configJsonFile)
        {
            String jsonStr = File.ReadAllText(configJsonFile);
            MongoDbConfig config = JsonConvert.DeserializeObject<MongoDbConfig>(jsonStr);
            return config;
        }
    }
    public class MediaRecordMongoDatabaseContext
    {
        readonly MongoDbConfig config;
        readonly MongoClient client;
        readonly IMongoDatabase database;

        public IMongoCollection<Image> Image { get; set; }

        public MediaRecordMongoDatabaseContext(MongoDbConfig config)
        {
            this.config = config;
            (string username, string password) = GetAuth(config.AuthenticationFile);
            string mongoDbConnection = string.Format(config.Uri, $"{username}:{password}@", config.DatabaseName);
            Console.WriteLine("linking to mongodb by: " + mongoDbConnection);
            this.client = new MongoClient();
            this.database = client.GetDatabase(config.DatabaseName);
            this.Image = this.database.GetCollection<Image>("Image");
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
