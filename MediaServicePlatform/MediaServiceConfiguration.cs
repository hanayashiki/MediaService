using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediaServicePlatform.ServiceFactory;
using Core;
using MediaServicePlatform;

namespace MediaServicePlatform
{
    public class MediaServiceConfiguration
    {
        public Core.Config CoreConfig { get; set; }
        public WebConfig WebConfig { get; set; }
        public IImageService ImageService { get; set; }

        public MediaServiceConfiguration()
        {
            CoreConfig = Config.GetConfig("C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/config.json");
            WebConfig = WebConfig.GetWebConfig("C:/Users/t-chwang/source/repos/ImageServingPlatform/MediaServicePlatform/webconfig.json");
            ImageService = ImageServiceFactory.GetImageService(CoreConfig);
        }
    }
}
