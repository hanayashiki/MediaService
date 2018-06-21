using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core;
using Core.DBManager;
using Core.MediaProcessors;
using Core.Models;
using Core.Storage;

namespace MediaServicePlatform.ServiceFactory
{
    public class ImageServiceFactory : ServiceFactory
    {
        public static ImageService GetImageService(BlobStorageConfig blobStorageConfig, MongoDbConfig mongoDbConfig)
        {
            BlobStorage blobStorage = new BlobStorage(blobStorageConfig);
            ImageProcessor imageProcessor = new ImageProcessor();
            //ImageDBManager imageDBManager = new ImageDBManager
            //    (new MediaRecordDatabaseContext());
            MediaRecordMongoDatabaseContext mongoDbContext = new MediaRecordMongoDatabaseContext(mongoDbConfig);
            ImageMongoDbManager imageDBManager = new ImageMongoDbManager(mongoDbContext);

            Core.ImageService imageService = new Core.ImageService(
                blobStorageConfig, imageProcessor, blobStorage, imageDBManager);
            return imageService;
        }

        public static ImageService GetImageServiceCached(BlobStorageConfig blobStorageConfig, 
            FileStorageConfig fileStorageConfig, MongoDbConfig mongoDbConfig)
        {
            BlobStorage blobStorage = new BlobStorage(blobStorageConfig);
            FileStorage fileStorage = new FileStorage(fileStorageConfig);
            CachedBlogStorage cachedBlogStorage = new CachedBlogStorage(blobStorage, fileStorage);
            ImageProcessor imageProcessor = new ImageProcessor();
            //ImageDBManager imageDBManager = new ImageDBManager
            //    (new MediaRecordDatabaseContext());
            MediaRecordMongoDatabaseContext mongoDbContext = new MediaRecordMongoDatabaseContext(mongoDbConfig);
            ImageMongoDbManager imageDBManager = new ImageMongoDbManager(mongoDbContext);

            Core.ImageService imageService = new Core.ImageService(
                blobStorageConfig, imageProcessor, cachedBlogStorage, imageDBManager
                );
            return imageService;
        }
    }
}
