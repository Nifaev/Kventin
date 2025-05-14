using Kventin.DataAccess.Domain;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Tools
{
    public interface IFileRecordFactory
    {
        public FileRecord Create(IFormFile file, string originalFileName, User uploadedByUser);
    }
}
