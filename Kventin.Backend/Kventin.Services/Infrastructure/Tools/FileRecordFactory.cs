using Kventin.DataAccess.Domain;
using Kventin.Services.Interfaces.Tools;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Infrastructure.Tools
{
    public class FileRecordFactory : IFileRecordFactory
    {
        public FileRecord Create(IFormFile file, string originalFileName, User uploadedByUser)
        {
            return new FileRecord
            {
                OriginalFileName = originalFileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedByUser = uploadedByUser,
                UploadedByUserId = uploadedByUser.Id,
                StorageFileName = $"{new Guid()}_{originalFileName}"
            };
        }
    }
}
