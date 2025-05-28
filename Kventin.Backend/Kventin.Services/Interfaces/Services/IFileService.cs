using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.Files;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IFileService
    {
        public Task UploadFileWithoutLinks(IFormFile file, long uploadedByUserId);
        public Task UploadFiles<T>(List<IFormFile> files, long uploadedByUserId, FileLinkType fileLinkType, long linkedEntityId = 0) where T : BaseEntity;
        public Task<DownloadFileDto> DownloadFile(long fileId);
        public Task DeleteFiles(List<long> fileIds);
        public Task<FileInfoDto> GetFileInfo(long fileId);
    }
}
