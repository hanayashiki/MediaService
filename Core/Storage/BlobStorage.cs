using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Storage
{
    public class BlobStorage : IStorage
    {
        private readonly BlobStorageConfig config;
        private readonly CloudStorageAccount storageAccount = null;
        private readonly CloudBlobClient cloudBlobClient = null;

        private ILogger logger;
        public BlobStorage(BlobStorageConfig config)
        {
            this.config = config;
            if (!CloudStorageAccount.TryParse(config.StorageConnectionString, out storageAccount))
            {
                throw new System.FormatException("storage connection string is invalid. ");
            }
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        private async Task<CloudBlockBlob> GetCloudBlockBlock(string blobName)
        {
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(config.ContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            return cloudBlockBlob;
        }

        private async Task<Uri> UploadBlobAsync(Stream stream, string blobName)
        {
            // TODO: Implement cancel
            CloudBlockBlob cloudBlockBlob = await GetCloudBlockBlock(blobName);
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            return cloudBlockBlob.Uri;
        }

        public async Task<Uri> UploadAsync(Stream stream, string blobName)
        {
            return await UploadBlobAsync(stream, blobName);
        }

        public async Task<byte[]> DownloadAsync(string blobName)
        {
            // TODO: Maybe implement cache
            CloudBlockBlob cloudBlockBlob = await GetCloudBlockBlock(blobName);
            MemoryStream stream = new MemoryStream();
            await cloudBlockBlob.DownloadToStreamAsync(stream);
            return stream.GetBuffer();
        }
        public bool Exists(string fileName)
        {
            throw new NotImplementedException();
        }
        public void UseLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public Uri Upload(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
