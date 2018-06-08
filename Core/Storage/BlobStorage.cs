using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Storage
{
    public class BlobStorage : IStorage
    {
        private readonly Config config;
        private readonly CloudStorageAccount storageAccount = null;
        private readonly CloudBlobClient cloudBlobClient = null;

        public BlobStorage(Config config)
        {
            this.config = config;
            if (!CloudStorageAccount.TryParse(config.StorageConnectionString, out storageAccount))
            {
                throw new System.FormatException("storage connection string is invalid. ");
            }
            cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        private async Task<Uri> UploadBlobAsync(Stream stream)
        {
            //TODO: correct Uri

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(config.ContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            string blobName = config.ContainerName + "_" + Guid.NewGuid().ToString();
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            return cloudBlockBlob.Uri;
        }

        public async Task<Uri> UploadAsync(Stream stream)
        {
            return await UploadBlobAsync(stream);
        }
    }
}
