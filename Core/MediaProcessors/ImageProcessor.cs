using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Core.Models;
using SixLabors;
using SI = SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Security.Cryptography;

namespace Core.MediaProcessors
{
    class ImageProcessor : IMediaProcessor<Image>
    {
        public void LoadInfoFromStream(Stream stream, ref Image image)
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
    }
}
