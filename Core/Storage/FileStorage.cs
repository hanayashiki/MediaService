using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Storage
{
    class FileStorageInfo
    {
        private long _totalSizeByte;
        public long TotalSizeByte {
            get {
                lock (_sync) { return _totalSizeByte; }
            }
            set {
                lock (_sync) { _totalSizeByte = value; }
            }
        }

        private object _sync = new object();
        public FileStorageInfo()
        {
            TotalSizeByte = 0;
        }
        public void DumpTo(string path)
        {
            lock (_sync)
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }
        }
        public void Load(string path)
        {
            lock (_sync)
            {
                string jsonStr = File.ReadAllText(path);
                JsonConvert.PopulateObject(jsonStr, this);
            }
        }
    }
    public class FileStorage : IStorage
    {
        private object _truncateSync = new object();

        private FileStorageInfo fileStorageInfo;
        private FileStorageConfig fileStorageConfig;

        private string infoFile;
        private string storageDir;

        private LockGroup<string> lockGroup;

        private ILogger logger;
        public FileStorage(FileStorageConfig fileStorageConfig)
        {
            this.fileStorageConfig = fileStorageConfig;
            lockGroup = new LockGroup<string>(fileStorageConfig.LockCount, s => s.GetHashCode());
            Init(fileStorageConfig);
        }
        public async Task<byte[]> DownloadAsync(string blobName)
        {
            return await Task.Run(() => { return File.ReadAllBytes(storageDir + blobName); });
        }
        public Uri Upload(Stream stream, string blobName)
        {
            logger.LogInformation($"FileStorage.Upload: for {blobName} starts");
            try
            {
                // if deleting, self lock
                logger.LogInformation($"FileStorage.Upload: for {blobName} trying to get lock");
                lock (lockGroup.GetLock(blobName))
                {
                    if (this.Exists(blobName))
                    {
                        return new Uri("file:///" + storageDir + blobName);
                    }

                    var fileStream = File.Create(storageDir + blobName);
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                    fileStream.Close();
                }
                FileInfo fileInfo = new FileInfo(storageDir + blobName);
                fileStorageInfo.TotalSizeByte += fileInfo.Length;
                fileStorageInfo.DumpTo(infoFile);
                if (fileStorageInfo.TotalSizeByte > fileStorageConfig.MaxSizeMB * 1024 * 1024)
                    TruncateStorage();
                return new Uri("file:///" + storageDir + blobName);
            }
            catch (IOException e)
            {
                logger.LogWarning("FileStorage.Upload IOException: " + e.Message);
                return new Uri("file:///" + storageDir + blobName);
            }
        }
        public bool Exists(string fileName)
        {
            lock (lockGroup.GetLock(fileName))
            {
                return File.Exists(storageDir + fileName);
            }
        }
        private void Init(FileStorageConfig fileStorageConfig)
        {
            //logger.LogInformation("FileStorage: Init starts");
            infoFile = fileStorageConfig.Directory + "filestorageinfo.json";
            storageDir = fileStorageConfig.Directory + "storage/";
            if (!Directory.Exists(fileStorageConfig.Directory))
            {
                Console.WriteLine("FileStorage::Init: No directory found, creating.");
                DirectoryInfo root = new DirectoryInfo(fileStorageConfig.Directory);
                DirectoryInfo storage = new DirectoryInfo(storageDir);
                root.Create();
                storage.Create();
            }
            fileStorageInfo = new FileStorageInfo();
            if (!File.Exists(infoFile))
            {
                fileStorageInfo.DumpTo(infoFile);
            } else
            {
                fileStorageInfo.Load(infoFile);
            }
            Console.WriteLine("FileStorage: Init ends");
        }
        private void TruncateStorage()
        {
            // TODO: Use LRU ?
            logger.LogInformation("FileStorage is truncating...");
            lock (_truncateSync)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(storageDir);
                IEnumerable<FileInfo> fileInfos = directoryInfo.EnumerateFiles();
                IEnumerator<FileInfo> enumerator = fileInfos.GetEnumerator();
                while (fileStorageInfo.TotalSizeByte > fileStorageConfig.MaxSizeMB * 1024 * 1024)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    string filepath = storageDir + enumerator.Current.Name;
                    logger.LogInformation($"FileStorage::TruncateStorage: Deleting: {filepath}");
                    lock (lockGroup.GetLock(enumerator.Current.Name))
                    {
                        if (this.Exists(enumerator.Current.Name)) { 
                            fileStorageInfo.TotalSizeByte -= enumerator.Current.Length;
                            File.Delete(filepath);
                        }
                    }
                    fileStorageInfo.DumpTo(infoFile);
                }
            }
        }
        public void UseLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public Task<Uri> UploadAsync(Stream stream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
