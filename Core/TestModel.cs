using Core.DBManager;
using Core.MediaProcessors;
using Core.Models;
using Core.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core
{
    class TestModel
    {
        static void TestDatabase()
        {
            var db = new MediaRecordDatabaseContext();
            db.Image.Add(new Image {
                Url = "https://avatars2.githubusercontent.com/u/26056783?s=460&v=4",
                Width = 243,
                Height = 243,
                MD5 = 233333333
            });

            var count = db.SaveChanges();

            foreach (var image in db.Image)
            {
                Console.WriteLine(" - {0} {1}", image.Id, image.Url);
            }

        }
        static void TestNoneImageRead()
        {
            string file_addr = "../../../Resource/testpic/none_jpg.txt";
            Image image = new Image();
            new ImageProcessor().LoadInfoFromStream(File.OpenRead(file_addr), ref image);
        }
        static void TestImageRead()
        {
            string file_addr = "../../../Resource/testpic/NieR-Emile.jpg";
            Image image = new Image();
            new ImageProcessor().LoadInfoFromStream(File.OpenRead(file_addr), ref image);
        }
        static void TestImageRead2()
        {
            string file_addr = "../../../Resource/testpic/github-octocat.png";
            Image image = new Image();
            new ImageProcessor().LoadInfoFromStream(File.OpenRead(file_addr), ref image);
        }
        static void TestGetConfig()
        {
            Config config = Config.GetConfig("C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/config.json");
            Console.WriteLine(config.StorageConnectionStringFile);
            Console.WriteLine(config.StorageConnectionString);
        }
        static void TestUploadBlob()
        {
            Config config = Config.GetConfig("C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/config.json");
            BlobStorage blobStorage = new BlobStorage(config);
            FileStream fs = new FileStream("C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/Resource/testpic/github-octocat.png", FileMode.Open);

            Task<Uri> t = blobStorage.UploadAsync(fs);
            t.Wait();
            Console.WriteLine(t.Result);
        }
        static async void TestUploadBinaryAsync()
        {
            Config config = Config.GetConfig("C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/config.json");
            BlobStorage blobStorage = new BlobStorage(config);
            ImageProcessor imageProcessor = new ImageProcessor();
            ImageDBManager imageDBManager = new ImageDBManager(new MediaRecordDatabaseContext());

            ImageService imageService = new ImageService(config, imageProcessor, blobStorage, imageDBManager);

            FileStream fs = new FileStream("C:/Users/t-chwang/source/repos/ImageServingPlatform/Core/Resource/testpic/github-octocat.png", FileMode.Open);
            byte[] fileContent = new BinaryReader(fs).ReadBytes((int)fs.Length);
            Console.WriteLine(fileContent.Length);
            fs.Close();

            await imageService.UploadBinaryAsync(fileContent);
            Console.WriteLine("End of TestUploadBinaryAsync. ");
        }
        public static void Main(String[] args)
        {
            // TestUploadBlob();
            TestUploadBinaryAsync();
            Console.ReadKey();
        }
    }
}
