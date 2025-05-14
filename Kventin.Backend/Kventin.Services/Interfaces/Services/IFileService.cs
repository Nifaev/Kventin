using Kventin.Services.Dtos.Files;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IFileService
    {
        public Task UploadFile(IFormFile file, int uploadedByUserId);
        public Task<DownloadFileDto> DownloadFile(int fileId);
        public Task DeleteFile(int fileId);
        public Task<FileInfoDto> GetFileInfo(int fileId);
    }
}
