using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Core.Storage
{
    class BlobStorage : Storage
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

        public async Task<Uri> UploadBlob(Stream stream)
        {
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(config.ContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            string blobName = config.ContainerName + "_" + Guid.NewGuid().ToString();
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            return cloudBlockBlob.Uri;
        }

    }
}
