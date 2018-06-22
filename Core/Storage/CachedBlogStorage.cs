using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Storage
{
    public class CachedBlogStorage : IStorage
    {
        private BlobStorage blobStorage;
        private FileStorage fileStorage;

        private ILogger logger;
        public CachedBlogStorage(BlobStorage blobStorage, FileStorage fileStorage)
        {
            this.blobStorage = blobStorage;
            this.fileStorage = fileStorage;
        }
        public async Task<byte[]> DownloadAsync(string fileName)
        {
            if (fileStorage.Exists(fileName))
            {
                logger.LogInformation($"{fileName} used fileStorage.");
                return await fileStorage.DownloadAsync(fileName);
            } else
            {
                logger.LogInformation($"{fileName} used blobStorage.");
                byte[] bytes = await blobStorage.DownloadAsync(fileName);
                fileStorage.Upload(new MemoryStream(bytes), fileName);
                return bytes;
            }
        }

        public bool Exists(string fileName)
        {
            throw new NotImplementedException();
        }

        public Uri Upload(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<Uri> UploadAsync(Stream stream, string fileName)
        {
            fileStorage.Upload(stream, fileName);
            return await blobStorage.UploadAsync(stream, fileName);
        }
        public void UseLogger(ILogger logger)
        {
            this.logger = logger;
            this.fileStorage.UseLogger(logger);
            this.blobStorage.UseLogger(logger);
        }
    }
}
