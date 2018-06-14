using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Storage
{
    public interface IStorage
    {
        Task<Uri> UploadAsync(Stream stream, string blobName);
        Task<byte[]> DownloadAsync(string blobName);
    }
}
