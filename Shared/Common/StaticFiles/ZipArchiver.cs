using System.IO;
using System.IO.Compression;

namespace Common.StaticFiles
{
    public static class ZipArchiver
    {
        public static byte[] GetZipArchive(BaseFileDto[] files)
        {
            byte[] archiveFile;

            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipEntry = archive.CreateEntry(file.FileDownloadName, CompressionLevel.Fastest);
                        using var zipStream = zipEntry.Open();
                        zipStream.Write(file.FileContent, 0, file.FileContent.Length);
                    }
                }

                archiveFile = archiveStream.ToArray();
            }

            return archiveFile;
        }
    }
}
