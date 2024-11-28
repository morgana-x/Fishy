using System.IO.Compression;

namespace Fishy.Helper
{
    static class GZip
    {
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                using MemoryStream inputStream = new(data);
                using GZipStream gzipStream = new(inputStream, CompressionMode.Decompress);
                using MemoryStream outputStream = new();
                gzipStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while decompressing GZIP: " + e.Message);
                return [];
            }
        }

        public static byte[] Compress(byte[] data)
        {
            try
            {
                using MemoryStream outputStream = new();
                using (GZipStream gzipStream = new(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return outputStream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while compressing GZIP: " + e.Message);
                return [];
            }
        }
    }
}
