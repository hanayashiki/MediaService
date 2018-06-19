using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class Utils
    {
        public static List<byte[]> File2Bytes(string path, int maxSegmentSize = int.MaxValue)
        {
            FileStream fileStreamRaw = new FileStream(path, FileMode.Open);
            return File2Bytes(fileStreamRaw, maxSegmentSize);
        }
        public static List<byte[]> File2Bytes(Stream stream, int maxSegmentSize = int.MaxValue)
        {
            BinaryReader fileStream = new BinaryReader(stream);
            List<byte[]> blockList = new List<byte[]>();
            while (true)
            {
                byte[] block = new byte[maxSegmentSize];
                int read = fileStream.Read(block, 0, block.Length);
                if (read < block.Length)
                    block = (byte[])ResizeArray(block, read);
                if (read > 0)
                {
                    blockList.Add(block);
                } else
                {
                    break;
                }
            }
            fileStream.Close();
            return blockList;
        }
        public static System.Array ResizeArray(System.Array oldArray, int newSize)
        {
            int oldSize = oldArray.Length;
            System.Type elementType = oldArray.GetType().GetElementType();
            System.Array newArray = System.Array.CreateInstance(elementType, newSize);
            int preserveLength = System.Math.Min(oldSize, newSize);
            if (preserveLength > 0)
                System.Array.Copy(oldArray, newArray, preserveLength);
            return newArray;
        }
    }
}
