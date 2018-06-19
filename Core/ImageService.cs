using Core.DBManager;
using Core.MediaProcessors;
using Core.Models;
using Core.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private ILogger Logger;

        private Stopwatch stopwatch = new Stopwatch();
        private Stopwatch stopwatch2 = new Stopwatch();

        public ImageService(Config config, ImageProcessor imageProcessor, 
            IStorage storage, IDBManager<Image> dBManager)
        {
            this.config = config;
            this.imageProcessor = imageProcessor;
            this.storage = storage;
            this.dbManager = dBManager;

            ILoggerFactory loggerFactory = new LoggerFactory()
              .AddConsole();

            ILogger logger = loggerFactory.CreateLogger<ImageService>();
        }

        public void UseLogger(ILogger logger)
        {
            this.Logger = logger;
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


            // Console.WriteLine("try get info stream");
            Stream infoSource = new MemoryStream(source);
            // Console.WriteLine("try get upload stream");
            Stream uploadSource = new MemoryStream(source);
            Image media = new Image();
            Image image;
            try
            {
                stopwatch.Restart();
                Task<Image> infoTask = imageProcessor.LoadInfoFromStreamAsync(infoSource, media);
                image = await infoTask;
                stopwatch.Stop();
                Logger.LogInformation($"Core.LoadInfoFromStreamAsync used {stopwatch.Elapsed.Milliseconds}ms. ");
            } catch (NotSupportedException)
            {
                return new UploadResult { Status = "invalid file format" };
            }
            image.BlobName = image.MD5.ToString("X") + GetFormatNameWithDot(fileName);
            stopwatch2.Start();
            Task<Uri> uploadTask = storage.UploadAsync(uploadSource, image.BlobName);           
    
            stopwatch.Restart();
            var id = dbManager.GetIdByMD5(image.MD5);
            string status = "ok";
            if (id == null)
            {
                // Console.WriteLine("Created: " + image.MD5);

                image = dbManager.Create(image);
                dbManager.SaveChanges();
            } else
            {
                image.Id = id;
                status = "duplicate";
            }
            stopwatch.Stop();
            Logger.LogInformation($"Core.dbManager used {stopwatch.Elapsed.Milliseconds}ms. ");

            Uri uri = await uploadTask;
            stopwatch2.Stop();
            Logger.LogInformation($"Core.UploadAsync used {stopwatch2.Elapsed.Milliseconds}ms. ");
            return new UploadResult { Status = status, Id = image.Id, UploadType = "image" };
        }
        public async Task<DownloadResult> DownloadBinaryAsync(string id)
        {
            (byte[] fileBytes, Image image) = await GetBlobAsync(id);
            // Console.WriteLine(fileBytes.Length);
            if (fileBytes == null)
            {
                return new DownloadResult { Status = "not found", FileBytes = null };
            }
            // TODO: exception here
            return new DownloadResult { Status = "ok", BlobName = image.BlobName, FileBytes = fileBytes };
        }

        public async Task<DownloadResult> DownloadAndCropBinaryAsync(string id, int xmin, int xmax, int ymin, int ymax)
        {
            stopwatch.Restart();
            (byte[] fileBytes, Image image) = await GetBlobAsync(id);
            stopwatch.Stop();
            Logger.LogInformation($"Core.DownloadAndCropBinaryAsync used {stopwatch.Elapsed.Milliseconds}ms. ");
            if (fileBytes == null)
            {
                return new DownloadResult { Status = "not found", FileBytes = null };
            }
            try
            {
                stopwatch.Restart();
                byte[] cropped = imageProcessor.CropImageToByte(new MemoryStream(fileBytes),
                                                                    xmin: xmin, xmax: xmax, ymin: ymin, ymax: ymax);
                stopwatch.Stop();
                Logger.LogInformation($"Core.CropImageToByte used {stopwatch.Elapsed.Milliseconds}ms. ");
                return new DownloadResult { Status = "ok", BlobName = image.BlobName, FileBytes = cropped };
            } catch (ArgumentOutOfRangeException)
            {
                return new DownloadResult { Status = "invalid arguments", BlobName = image.BlobName, FileBytes = null };
            }
        }

        public async Task<InfoResult> GetInfoAsync(string id)
        {
            Image image = await dbManager.GetRecordByIdAsync(id);
            if (image == null)
            {
                return new InfoResult { Status = "not found" };
            }
            return new InfoResult(image) { Status = "ok"};
        }

        private async Task<(byte[], Image)> GetBlobAsync(string id)
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
