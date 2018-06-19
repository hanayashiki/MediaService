using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Storage
{
    class MongoDbStorage : IStorage
    {
        public Task<byte[]> DownloadAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<Uri> UploadAsync(Stream stream, string blobName)
        {
            throw new NotImplementedException();
        }
    }
}
