using Core;
using Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Runnables
{
    class Program
    {
        static Config config = Config.GetConfig(@"C:\Users\t-chwang\source\repos\ImageServingPlatform\ImageServingPlatform\Service\service_config.json");
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
        static void TestDatabase()
        {

        }
        static void Main(string[] args)
        {
            TestDatabase();
            Console.ReadKey();
        }
    }
}
