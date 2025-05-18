using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Tools
{
    public interface IFileRecordFactory
    {
        public FileRecord Create<T>(IFormFile file, string originalFileName, User uploadedByUser, FileLinkType fileLinkType, T? linkedEntity) where T : BaseEntity;
    }
}
