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
        public static ImageService GetImageService(Config config)
        {
            BlobStorage blobStorage = new BlobStorage(config);
            ImageProcessor imageProcessor = new ImageProcessor();
            ImageDBManager imageDBManager = new ImageDBManager
                (new MediaRecordDatabaseContext());

            ImageService imageService = new ImageService(
                config, imageProcessor, blobStorage, imageDBManager);
            return imageService;
        }
    }
}
