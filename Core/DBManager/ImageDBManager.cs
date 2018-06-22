using Core.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DBManager
{
    [Obsolete]
    public class ImageDBManager : IDBManager<Image>
    {
        MediaRecordDatabaseContext context;
        public ImageDBManager(MediaRecordDatabaseContext context)
        {
            this.context = context;
        }
        public Image Create(Image image)
        {
            EntityEntry<Image> entityEntry = context.Image.Add(image);
            return entityEntry.Entity;
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
        public String GetIdByMD5(long MD5)
        {
            IEnumerable<string> idQuery =
                from image in context.Image
                where image.MD5 == MD5
                select image.Id;
            foreach (string id in idQuery)
            {
                return id;
            }
            return null;
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
        public Image GetRecordById(string id)
        {
            return context.Find<Image>(id);
        }
        public Task<Image> GetRecordByIdAsync(string id)
        {
            return context.FindAsync<Image>(id);
        }
    }
}
