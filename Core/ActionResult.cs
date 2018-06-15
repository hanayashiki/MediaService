using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core
{
    public class CoreActionResult
    {
        public string Status { get; set; }
    }

    public class UploadResult : CoreActionResult
    {
        public string UploadType { get; set; }
        public string Id { get; set; }

    }

    public class DownloadResult : CoreActionResult
    {
        public string BlobName { get; set; }
        public Byte [] FileBytes { get; set; }
    }

    public class InfoResult : CoreActionResult
    {
        public ImageInfo ImageInfo { get; set; }
        public InfoResult(Image image)
        {
            ImageInfo = new ImageInfo(image);
        }
        public InfoResult()
        {
        }
    }

    public class ImageInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("MD5")]
        public string MD5 { get; set; }
        [JsonProperty("blobName")]
        public string BlobName { get; set; }
        public ImageInfo(Image image)
        {
            Id = image.Id;
            Width = image.Width;
            Height = image.Height;
            MD5 = image.MD5.ToString("x16");
            BlobName = image.BlobName;
        }
    }
}