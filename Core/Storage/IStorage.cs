using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Storage
{
    public interface IStorage
    {
        Task<Uri> UploadAsync(Stream stream, string fileName);
        Uri Upload(Stream stream, string fileName);
        Task<byte[]> DownloadAsync(string fileName);

        Boolean Exists(string fileName);
        void UseLogger(ILogger logger);
    }
}
