using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core
{
    public class BlobStorageConfig
    {
        public string StorageConnectionStringFile { get; set; }
        public int MaxSegmentSize { get; set; }
        public string StorageConnectionString { get; set; }
        public string ContainerName { get; set; }

        public static BlobStorageConfig GetConfig(String configJsonFile)
        {
            String jsonStr = File.ReadAllText(configJsonFile);
            BlobStorageConfig config = JsonConvert.DeserializeObject<BlobStorageConfig>(jsonStr);
            String storageConnectionStringFileStr = File.ReadAllText(config.StorageConnectionStringFile);
            config.StorageConnectionString = 
                JObject.Parse(storageConnectionStringFileStr)["StorageConnectionString"].ToString();
            return config;
        }
    }
}
