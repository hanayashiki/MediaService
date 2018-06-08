using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediaServicePlatform.ServiceFactory;
using Core;

namespace MediaServicePlatform
{
    public class MediaServiceConfiguration
    {
        public Config Config { get; set; }
        public IImageService ImageService { get; set; }
        public MediaServiceConfiguration()
        {
            Config = Config.GetConfig("C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/config.json");
            ImageService = ImageServiceFactory.GetImageService(Config);
        }
    }
}
