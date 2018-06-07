using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.MediaProcessors
{
    interface IMediaProcessor<T>
    {
        void LoadInfoFromStream(Stream stream, ref T media);
        long GetHash(Stream fs);

    }
}
