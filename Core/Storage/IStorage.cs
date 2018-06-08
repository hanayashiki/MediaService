using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.Storage
{
    interface IStorage
    {
        Task<Uri> UploadAsync(Stream stream);
    }
}
