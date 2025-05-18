using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.Files;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IFileService
    {
        public Task UploadFileWithoutLinks(IFormFile file, int uploadedByUserId);
        public Task UploadFile<T>(IFormFile file, int uploadedByUserId, FileLinkType fileLinkType, int linkedEntityId = 0) where T : BaseEntity;
        public Task<DownloadFileDto> DownloadFile(int fileId);
        public Task DeleteFile(int fileId);
        public Task<FileInfoDto> GetFileInfo(int fileId);
    }
}
