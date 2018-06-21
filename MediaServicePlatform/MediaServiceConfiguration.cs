using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediaServicePlatform.ServiceFactory;
using Core;
using MediaServicePlatform;
using Core.DBManager;

namespace MediaServicePlatform
{
    [Obsolete]
    public class MediaServiceConfiguration
    {
        public Core.BlobStorageConfig CoreConfig { get; set; }
        public WebConfig WebConfig { get; set; }
        public IImageService ImageService { get; set; }

        public MediaServiceConfiguration()
        {
            CoreConfig = BlobStorageConfig.GetConfig("./Resource/blobstorageconfig.json");
            WebConfig = WebConfig.GetWebConfig("./webconfig.json");
            MongoDbConfig mongoDbConfig = MongoDbConfig.GetMongoDbConfig("./Resource/mongodbconfig.json");
            ImageService = ImageServiceFactory.GetImageService(CoreConfig, mongoDbConfig);
        }
    }
}
