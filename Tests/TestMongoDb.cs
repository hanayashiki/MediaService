using System;
using System.Collections.Generic;
using System.Text;

using Core;
using Core.DBManager;
using System.IO;
using Xunit.Abstractions;
using Xunit;
using Core.Models;

namespace Tests
{
    public class TestMongoDb
    {
        ITestOutputHelper output;
        MediaRecordMongoDatabaseContext context;
        ImageMongoDbManager manager;
        public TestMongoDb(ITestOutputHelper output)
        {
            MongoDbConfig mongoDbConfig = MongoDbConfig.GetMongoDbConfig(@"C:\Users\t-chwang\source\repos\ImageServingPlatform\Tests\mongodbconfig.json");
            this.context = new MediaRecordMongoDatabaseContext(mongoDbConfig);
            manager = new ImageMongoDbManager(context);
            this.output = output;
        }

        [Fact]
        public void TestCreate()
        {
            Image image = new Image { Width = 500, Height = 500, BlobName = "Shit", MD5 = 123 };
            image = manager.Create(image);
            output.WriteLine(image.Id);
        }
        [Fact]
        public void TestGetIdByMD5()
        {
            String str = manager.GetIdByMD5(0xdead);
            Assert.Null(str);
            str = manager.GetIdByMD5(123);
            Assert.NotNull(str);
        }
        [Fact]
        public void TestCheckDuplicate()
        {
            Assert.True(manager.CheckDuplicate(new Image { MD5 = 123 }));
            Assert.False(manager.CheckDuplicate(new Image { MD5 = 0xdead }));
        }
    }
}
