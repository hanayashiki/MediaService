using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaServicePlatform
{
    public class WebConfig
    {
        public string HostName { get; set; }
        public string Protocol { get; set; }
        public static WebConfig GetWebConfig(string filePath)
        {
            return JsonConvert.DeserializeObject<WebConfig>(File.ReadAllText(filePath));
        }
    }
}
