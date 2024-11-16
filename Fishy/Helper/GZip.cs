using System.IO.Compression;

namespace Fishy.Helper
{
    static class GZip
    {
        public static byte[] Decompress(byte[] data)
        {
            using MemoryStream inputStream = new(data);
            using GZipStream gzipStream = new(inputStream, CompressionMode.Decompress);
            using MemoryStream outputStream = new();
            gzipStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }

        public static byte[] Compress(byte[] data)
        {
            using MemoryStream outputStream = new();
            using (GZipStream gzipStream = new(outputStream, CompressionMode.Compress))
            {
                gzipStream.Write(data, 0, data.Length);
            }
            return outputStream.ToArray();
        }
    }
}
