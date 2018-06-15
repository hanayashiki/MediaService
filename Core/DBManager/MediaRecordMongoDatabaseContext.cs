using Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

using Newtonsoft.Json;
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
            this.client = new MongoClient(config.Uri);
            this.database = client.GetDatabase(config.DatabaseName);
            this.Image = this.database.GetCollection<Image>("Image");
        }

        public void InitializeCounter()
        {

        } 
    }
}
