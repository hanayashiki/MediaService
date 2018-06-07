using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core
{
    class ImageService : IMediaService
    {
        readonly Config config;
        ImageService(Config config)
        {
            this.config = config;
        } 
        UploadResult IMediaService.UploadBinary(Stream stream)
        {
            // List<byte[]> blockList = Utils.File2Bytes(stream, config.MaxSegmentSize);
            return null;
        }
    }
}
