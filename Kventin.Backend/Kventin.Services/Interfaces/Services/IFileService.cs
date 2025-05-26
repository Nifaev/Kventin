using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.Files;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IFileService
    {
        public Task UploadFileWithoutLinks(IFormFile file, int uploadedByUserId);
        public Task UploadFiles<T>(List<IFormFile> files, int uploadedByUserId, FileLinkType fileLinkType, int linkedEntityId = 0) where T : BaseEntity;
        public Task<DownloadFileDto> DownloadFile(int fileId);
        public Task DeleteFiles(List<int> fileIds);
        public Task<FileInfoDto> GetFileInfo(int fileId);
    }
}
