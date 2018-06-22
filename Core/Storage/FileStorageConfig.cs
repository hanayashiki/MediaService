using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Storage
{
    public class FileStorageConfig
    {
        public string Directory { get; set; }
        public int MaxSizeMB { get; set; }
        public int LockCount { get; set; }

        public static FileStorageConfig GetConfig(String configJsonFile)
        {
            String jsonStr = File.ReadAllText(configJsonFile);
            return JsonConvert.DeserializeObject<FileStorageConfig>(jsonStr);
        }
    }
}
