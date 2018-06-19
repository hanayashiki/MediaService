using Core;
using Core.DBManager;
using Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Runnables
{
    class Program
    {
        static BlobStorageConfig config = BlobStorageConfig.GetConfig(@"C:\Users\t-chwang\source\repos\ImageServingPlatform\ImageServingPlatform\Service\service_config.json");
        static readonly String testFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
        static void DebugFile2Bytes()
        {
            FileStream file = File.Create(testFolder + "temp.file");
            Console.WriteLine(file == null);
            file.Write(new byte[2 * config.MaxSegmentSize + 1], 0, 2 * config.MaxSegmentSize + 1);
            file.Close();

            List<byte[]> blockList = Utils.File2Bytes(testFolder + "temp.file", config.MaxSegmentSize);
            Console.WriteLine(blockList.Count);
            // File.Delete(testFolder + "temp.file");

            Console.ReadKey();
        }
        static void TestMongoModel()
        {
            MediaRecordMongoDatabaseContext context;
            ImageMongoDbManager manager;
            MongoDbConfig mongoDbConfig = MongoDbConfig.GetMongoDbConfig(@"C:\Users\t-chwang\source\repos\ImageServingPlatform\Core\DBManager\resource\mongodbconfig.json");
            context = new MediaRecordMongoDatabaseContext(mongoDbConfig);
            manager = new ImageMongoDbManager(context);

            Image image = new Image { Width = 500, Height = 500, BlobName = "Shit", MD5 = 123 };
            image = manager.Create(image);
            Console.WriteLine(image.Id);
        }

        static void TestMongoModelGetIdByMD5()
        {
            MediaRecordMongoDatabaseContext context;
            ImageMongoDbManager manager;
            MongoDbConfig mongoDbConfig = MongoDbConfig.GetMongoDbConfig(@"C:\Users\t-chwang\source\repos\ImageServingPlatform\Core\DBManager\resource\mongodbconfig.json");
            context = new MediaRecordMongoDatabaseContext(mongoDbConfig);
            manager = new ImageMongoDbManager(context);

            String str = manager.GetIdByMD5(0xdead);

            str = manager.GetIdByMD5(123);
            Console.WriteLine(str);
        }
        static void Main(string[] args)
        {
            TestMongoModelGetIdByMD5();
            Console.ReadKey();
        }
    }
}
