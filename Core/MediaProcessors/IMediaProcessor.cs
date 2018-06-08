using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Core.MediaProcessors
{
    public interface IMediaProcessor<T>
    {
        Task<T> LoadInfoFromStreamAsync(Stream stream, T image);
        long GetHash(Stream fs);

    }
}
