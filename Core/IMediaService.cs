using System;
using System.IO;

namespace Core
{
    public interface IMediaService
    {
        UploadResult UploadBinary(Stream stream);
    }
}
