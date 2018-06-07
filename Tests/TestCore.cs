using System;
using Xunit;
using System.Diagnostics;

using Core;
using System.IO;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Tests
{
    public class TestService
    {
        ITestOutputHelper output;
        readonly Config config;
        readonly String testFolder;
        public TestService(ITestOutputHelper output)
        {
            config = Config.GetConfig(@"C:\Users\t-chwang\source\repos\ImageServingPlatform\Core\config.json");
            testFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";
            this.output = output;
        }

        [Fact]
        public void TestConfig()
        {
            Assert.Equal(4194304, config.MaxSegmentSize);
        }

        [Fact]
        public void TestFile2Bytes()
        {
            BinaryWriter file = new BinaryWriter(File.Create(testFolder + "temp.file"));
            file.Write(new byte[2 * config.MaxSegmentSize + 1], 0, 2 * config.MaxSegmentSize + 1);
            file.Close();

            output.WriteLine(testFolder);

            List<byte[]> blockList = Utils.File2Bytes(testFolder + "temp.file", config.MaxSegmentSize);
            Assert.Equal(4194304, config.MaxSegmentSize);
            Assert.Equal(3, blockList.Count);
            Assert.Single(blockList[2]);
            File.Delete(testFolder + "temp.file");
        }
    }
}
