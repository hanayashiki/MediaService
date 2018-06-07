using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core
{
    class TestModel
    {
        static void TestDatabase()
        {
            var db = new MediaRecordDatabaseContext();
            db.Image.Add(new Image {
                Id = 0,
                Url = "https://avatars2.githubusercontent.com/u/26056783?s=460&v=4",
                Width = 243,
                Height = 243,
                MD5 = 233333333
            });

            var count = db.SaveChanges();

            foreach (var image in db.Image)
            {
                Console.WriteLine(" - {0}", image.Url);
            }

        }
        public static void Main(String[] args)
        {
            TestDatabase();
            Console.ReadKey();
        }
    }
}
