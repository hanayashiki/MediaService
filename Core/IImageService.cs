using System;
using System.IO;
using System.Threading.Tasks;

namespace Core
{
    public interface IImageService
    {
        //Task<UploadResult> UploadBinaryAsync(MemoryStream source);
        Task<UploadResult> UploadBinaryAsync(byte[] source);
    }

}
