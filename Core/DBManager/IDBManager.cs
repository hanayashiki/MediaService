using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DBManager
{
    public interface IDBManager<MediaType>
    {
        void Create(MediaType media);
        void UpdateById(int id, MediaType media);
        void DeleteById(int id);
        bool CheckDuplicate(MediaType media);
        void SaveChanges();
    }
}
