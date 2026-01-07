namespace Common.StaticFiles
{
    public abstract class BaseFileDto
    {
        public abstract byte[] FileContent { get; set; }

        public abstract string ContentType { get; set; }

        public abstract string FileDownloadName { get; set; }
    }
}
