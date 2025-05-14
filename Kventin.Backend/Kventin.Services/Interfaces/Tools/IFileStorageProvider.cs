namespace Kventin.Services.Interfaces.Tools
{
    public interface IFileStorageProvider
    {
        Task UploadFileAsync(Stream stream, string storageFileName,string contentType);
        Task DeleteFileAsync(string storageFileName);
        Task<MemoryStream> DownloadFileAsync(string storageFileName);
    }
}
