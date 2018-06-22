using System;
using System.Threading.Tasks;

namespace Core.DBManager
{
    public interface IDBManager<MediaType>
    {
        MediaType Create(MediaType media);
        MediaType GetRecordById(string id);
        Task<MediaType> GetRecordByIdAsync(string id);
        void UpdateById(int id, MediaType media);
        void DeleteById(int id);
        bool CheckDuplicate(MediaType media);
        String GetIdByMD5(long MD5);
        void SaveChanges();
    }
}
