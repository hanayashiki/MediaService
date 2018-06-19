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
    public class MediaRecordMongoDatabaseContext : MongoDatabaseContext
    {

        public IMongoCollection<Image> Image { get; set; }

        public MediaRecordMongoDatabaseContext(MongoDbConfig config) : base(config)
        {
            this.Image = this.database.GetCollection<Image>("Image");
        }
    
    }
}
