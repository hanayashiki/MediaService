﻿using Core.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Core.DBManager
{
    public class ImageMongoDbManager : IDBManager<Image>
    {
        MediaRecordMongoDatabaseContext context;
        public ImageMongoDbManager(MediaRecordMongoDatabaseContext context)
        {
            this.context = context;
        }
        public bool CheckDuplicate(Image media)
        {
            return context.Image.Find(m => m.MD5 == media.MD5).Any();
        }

        public Image Create(Image media)
        {
            //var bsonDoc = media.ToBsonDocument();
            context.Image.InsertOne(media);
            return media;
        }

        void IDBManager<Image>.DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public String GetIdByMD5(long MD5)
        {
            var sequence = context.Image.Find(m => m.MD5 == MD5).ToList();
            if (sequence.Count() == 0)
            {
                return null;
            } else
            {
                return sequence[0].Id;
            }
        }

        public Image GetRecordById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Image> GetRecordByIdAsync(string id)
        {
            var find = await context.Image.FindAsync(m => m.Id == id);
            var sequence = find.ToList();
            if (sequence.Count() == 0)
            {
                return null;
            }
            else
            {
                return sequence[0];
            }
        }

        public void SaveChanges()
        {
            
        }

        public void UpdateById(int id, Image media)
        {
            throw new NotImplementedException();
        }
    }
}
