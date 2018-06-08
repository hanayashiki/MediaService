using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DBManager
{
    public class ImageDBManager : IDBManager<Image>
    {
        MediaRecordDatabaseContext context;
        public ImageDBManager(MediaRecordDatabaseContext context)
        {
            this.context = context;
        }
        public void Create(Image image)
        {
            context.Image.Add(image);
        }
        public void UpdateById(int id, Image image)
        {
            throw new NotImplementedException();
        }
        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }
        public bool CheckDuplicate(Image media)
        {
            return context.Image.Any(m => m.MD5 == media.MD5);
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
