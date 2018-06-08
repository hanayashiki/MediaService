using Core.DBManager;
using Core.MediaProcessors;
using Core.Models;
using Core.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ImageService: IImageService
    {
        readonly Config config;
        readonly IMediaProcessor<Image> imageProcessor;
        readonly IStorage storage;
        readonly IDBManager<Image> dbManager;
        public ImageService(Config config, IMediaProcessor<Image> imageProcessor, 
            IStorage storage, IDBManager<Image> dBManager)
        {
            this.config = config;
            this.imageProcessor = imageProcessor;
            this.storage = storage;
            this.dbManager = dBManager;
        } 
        public async Task<UploadResult> UploadBinaryAsync(byte[] source)
        {
            //Console.WriteLine("try get info stream");
            //Stream infoSource = new MemoryStream(source.GetBuffer());
            //Console.WriteLine("try get upload stream");
            //Stream uploadSource = new MemoryStream(source.GetBuffer());
            Console.WriteLine("try get info stream");
            Stream infoSource = new MemoryStream(source);
            Console.WriteLine("try get upload stream");
            Stream uploadSource = new MemoryStream(source);
            Image media = new Image();
            Task<Image> infoTask = imageProcessor.LoadInfoFromStreamAsync(infoSource, media);
            Task<Uri> uploadTask = storage.UploadAsync(uploadSource);

            Image image = await infoTask;

            if (dbManager.CheckDuplicate(image))
            {
                // TODO
                Console.WriteLine("Fuck! md5 duplicate. ");
            }
            else
            {
                Console.WriteLine("Created: " + image.MD5);
                Uri uri = await uploadTask;
                image.Url = uri.OriginalString;
                dbManager.Create(image);
                dbManager.SaveChanges();
            }
            Console.WriteLine("End.");
            return null;
        }
    }
}
