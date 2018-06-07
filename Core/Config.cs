using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core
{
    public class Config
    {
        public String Domain { get; set; }
        public String SAS_Signature { get; set; }
        public int MaxSegmentSize { get; set; }

        public static Config GetConfig(String configJsonFile)
        {
            String jsonStr = File.ReadAllText(configJsonFile);
            return JsonConvert.DeserializeObject<Config>(jsonStr);
        }
    }
}
