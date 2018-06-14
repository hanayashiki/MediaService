using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.DBManager
{
    public interface IDBManager<MediaType>
    {
        MediaType Create(MediaType media);
        MediaType GetRecordById(int id);
        Task<MediaType> GetRecordByIdAsync(int id);
        void UpdateById(int id, MediaType media);
        void DeleteById(int id);
        bool CheckDuplicate(MediaType media);
        int? GetIdByMD5(long MD5);
        void SaveChanges();
    }
}
