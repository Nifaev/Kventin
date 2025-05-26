using Kventin.Services.Dtos.Files;

namespace Kventin.Services.Interfaces.Tools
{
    public interface IFileStorageProvider
    {
        Task UploadFileAsync(Stream stream, string storageFileName,string contentType);
        Task UploadFilesAsync(List<UploadFileDto> files);
        Task DeleteFileAsync(string storageFileName);
        Task DeleteFilesAsync(List<string> storageFileNames);
        Task<MemoryStream> DownloadFileAsync(string storageFileName);
    }
}
