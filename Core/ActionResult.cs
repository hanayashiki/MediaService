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
        public int Id { get; set; }

    }

    public class DownloadResult : CoreActionResult
    {
        public string BlobName { get; set; }
        public Byte [] FileBytes { get; set; }
    }
}