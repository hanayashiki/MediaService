using System;
using System.IO;
using System.Threading.Tasks;

namespace Core
{
    public interface IImageService
    {
        //Task<UploadResult> UploadBinaryAsync(MemoryStream source);
        Task<UploadResult> UploadBinaryAsync(byte[] source, string fileName);
        Task<DownloadResult> DownloadBinaryAsync(int id);

        Task<DownloadResult> DownloadAndCropBinaryAsync(int id, int xmin, int xmax, int ymin, int ymax);
    }

}
