using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;
using System;
using System.Diagnostics;
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
            //SI.Formats.IImageFormat format;
            //SI.Image<Rgba32> sImage;

            //try
            //{
            //    Stopwatch stopWatch = new Stopwatch();
            //    stopWatch.Start();
            //    sImage = SI.Image.Load(stream, out format);



            //    Console.Write($"SI.Image.Load used {stopWatch.Elapsed.Milliseconds}ms.");
            //} catch (ImageFormatException)
            //{
            //    throw new NotSupportedException();
            //}

            //image.Height = sImage.Height;
            //image.Width = sImage.Width;
            //image.MD5 = GetHash(stream);

            try
            {
                using (System.Drawing.Image tif = System.Drawing.Image.FromStream(stream: stream,
                                                         useEmbeddedColorManagement: false,
                                                         validateImageData: false))
                {
                    float width = tif.PhysicalDimension.Width;
                    float height = tif.PhysicalDimension.Height;

                    image.Height = (int)height;
                    image.Width = (int)width;
                    image.MD5 = GetHash(stream);
                    image.Format = GetImageFormatFromStream(stream);
                    Console.WriteLine("Got image format: " + image.Format);
                }
            } catch (ArgumentException)
            {
                throw new NotSupportedException();
            }

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

        private string GetImageFormatFromStream(Stream stream)
        {
            stream.Position = 0;
            byte[] sb = new byte[2];  //这次读取的就是直接0-1的位置长度了.
            stream.Read(sb, 0, sb.Length);
            stream.Position = 0;
            //根据文件头判断
            string strFlag = sb[0].ToString() + sb[1].ToString();
            //察看格式类型
            switch (strFlag)
            {
                //JPG格式
                case "255216":
                    return ".jpg";
                //GIF格式
                case "7173":
                    return ".gif";
                //BMP格式
                case "6677":
                    return ".bmp";
                //PNG格式
                case "13780":
                    return ".png";
                //其他格式
                default:
                    return "";
            }
        }
        public byte[] CropImageToByte(Stream stream, int xmin, int xmax, int ymin, int ymax)
        {
            SI.Image<Rgba32> sImage = SI.Image.Load(stream, out SI.Formats.IImageFormat format);
            try
            {
                sImage.Mutate(m => m.Crop(new Rectangle(xmin, ymin, xmax - xmin, ymax - ymin)));
            } catch (ImageProcessingException) {
                throw new ArgumentOutOfRangeException();
            }
            System.Console.WriteLine(sImage.Width);
            System.Console.WriteLine(sImage.Height);
            MemoryStream memoryStream = new MemoryStream();
            sImage.Save(memoryStream, format);
            return memoryStream.GetBuffer();
        }
    }
}
