namespace Kventin.Services.Dtos.Files
{
    public class UploadFileDto
    {
        public required Stream Stream { get; set; }
        public required string StorageFileName { get; set; }
        public required string ContentType { get; set; }
    }
}
