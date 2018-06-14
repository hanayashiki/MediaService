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
        readonly ImageProcessor imageProcessor;
        readonly IStorage storage;
        readonly IDBManager<Image> dbManager;
        public ImageService(Config config, ImageProcessor imageProcessor, 
            IStorage storage, IDBManager<Image> dBManager)
        {
            this.config = config;
            this.imageProcessor = imageProcessor;
            this.storage = storage;
            this.dbManager = dBManager;
        }

        public string GetFormatNameWithDot(string fileName)
        {
            string[] splitted = fileName.Split(".");
            if (splitted.Length > 1)
            {
                return "." + splitted[splitted.Length - 1];
            } else
            {
                return "";
            }
        }
        public async Task<UploadResult> UploadBinaryAsync(byte[] source, string fileName)
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
            Image image = await infoTask;
            image.BlobName = image.MD5.ToString("X") + GetFormatNameWithDot(fileName);
            Task<Uri> uploadTask = storage.UploadAsync(uploadSource, image.BlobName);
            Uri uri = await uploadTask;

            var id = dbManager.GetIdByMD5(image.MD5);
            string status = "ok";
            if (id == null)
            {
                Console.WriteLine("Created: " + image.MD5);
                image = dbManager.Create(image);
                dbManager.SaveChanges();
            } else
            {
                image.Id = (int)id;
                status = "duplicate";
            }

            Console.WriteLine("End.");
            return new UploadResult { Status = status, Id = image.Id, UploadType = "image" };
        }
        public async Task<DownloadResult> DownloadBinaryAsync(int id)
        {
            (byte[] fileBytes, Image image) = await GetBlobAsync(id);
            if (fileBytes == null)
            {
                new DownloadResult { Status = "not found", FileBytes = null };
            }
            // TODO: exception here
            return new DownloadResult { Status = "ok", BlobName = image.BlobName, FileBytes = fileBytes };
        }

        public async Task<DownloadResult> DownloadAndCropBinaryAsync(int id, int xmin, int xmax, int ymin, int ymax)
        {
            (byte[] fileBytes, Image image) = await GetBlobAsync(id);
            if (fileBytes == null)
            {
                new DownloadResult { Status = "not found", FileBytes = null };
            }
            byte[] cropped = imageProcessor.CropImageToByte(new MemoryStream(fileBytes),
                                                                xmin: xmin, xmax: xmax, ymin: ymin, ymax: ymax);
            return new DownloadResult { Status = "ok", BlobName = image.BlobName, FileBytes = cropped };
        }

        private async Task<(byte[], Image)> GetBlobAsync(int id)
        {
            Image image = await dbManager.GetRecordByIdAsync(id);
            if (image == null)
            {
                return (null, null);
            }
            byte[] fileBytes = await storage.DownloadAsync(image.BlobName);
            return (fileBytes, image);
        }
    }
}
