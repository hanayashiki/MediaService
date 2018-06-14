using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SI = SixLabors.ImageSharp;

namespace Core.MediaProcessors
{
    public class ImageProcessor : IMediaProcessor<Models.Image>
    {
        public async Task<Models.Image> LoadInfoFromStreamAsync(Stream stream, Models.Image image)
        {
            await Task.Run(() => LoadInfoFromStream(stream, ref image));
            return image;
        }
        public void LoadInfoFromStream(Stream stream, ref Models.Image image)
        {
            SI.Formats.IImageFormat format;
            SI.Image<Rgba32> sImage = SI.Image.Load(stream, out format);

            image.Height = sImage.Height;
            image.Width = sImage.Width;
            image.MD5 = GetHash(stream);
        }
        public long GetHash(Stream fs)
        {
            using (var ms = new MemoryStream())
            {
                fs.Position = 0;
                fs.CopyTo(ms);
                using (var hashAlgorithm = MD5.Create())
                {
                    byte[] md5 = hashAlgorithm.ComputeHash(ms.ToArray());
                    long low64 = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        low64 += ((long)md5[i]) << (8 * i);
                    }
                    return low64;
                }
            }
        }
        public byte[] CropImageToByte(Stream stream, int xmin, int xmax, int ymin, int ymax)
        {
            SI.Image<Rgba32> sImage = SI.Image.Load(stream, out SI.Formats.IImageFormat format);
            sImage.Mutate(m => m.Crop(new Rectangle(xmin, ymin, xmax - xmin, ymax - ymin)));
            System.Console.WriteLine(sImage.Width);
            System.Console.WriteLine(sImage.Height);
            MemoryStream memoryStream = new MemoryStream();
            sImage.Save(memoryStream, format);
            return memoryStream.GetBuffer();
        }
    }
}
