using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.DBManager;
using Core.MediaProcessors;
using Core.Models;
using Core.Storage;

namespace MediaServicePlatform.ServiceFactory
{
    public class ImageServiceFactory : ServiceFactory
    {
        public static Core.ImageService GetImageService(Core.Config config)
        {
            BlobStorage blobStorage = new BlobStorage(config);
            ImageProcessor imageProcessor = new ImageProcessor();
            //ImageDBManager imageDBManager = new ImageDBManager
            //    (new MediaRecordDatabaseContext());
            MongoDbConfig mongoDbConfig = MongoDbConfig.GetMongoDbConfig(@"C:\Users\t-chwang\source\repos\ImageServingPlatform\Core\DBManager\resource\mongodbconfig.json");
            MediaRecordMongoDatabaseContext mongoDbContext = new MediaRecordMongoDatabaseContext(mongoDbConfig);
            ImageMongoDbManager imageDBManager = new ImageMongoDbManager(mongoDbContext);

            Core.ImageService imageService = new Core.ImageService(
                config, imageProcessor, blobStorage, imageDBManager);
            return imageService;
        }
    }
}
