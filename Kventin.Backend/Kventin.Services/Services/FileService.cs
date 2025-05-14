using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Files;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Interfaces.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class FileService(KventinContext db,
        IFileStorageProvider fileStorageProvider,
        IFileRecordFactory fileRecordFactory) : IFileService
    {
        private readonly KventinContext _db = db;
        private readonly IFileStorageProvider _fileStorageProvider = fileStorageProvider;
        private readonly IFileRecordFactory _fileRecordFactory = fileRecordFactory;

        public async Task DeleteFile(int fileId)
        {
            var fileRecord = await _db.FileRecords.FindAsync(fileId);

            await _fileStorageProvider.DeleteFileAsync(fileRecord!.StorageFileName);
        
            _db.FileRecords.Remove(fileRecord);

            await _db.SaveChangesAsync();
        }

        public async Task<DownloadFileDto> DownloadFile(int fileId)
        {
            var fileRecord = await _db.FileRecords.FindAsync(fileId);

            var file = await _fileStorageProvider.DownloadFileAsync(fileRecord!.StorageFileName);

            var dto = new DownloadFileDto
            {
                File = file,
                ContentType = fileRecord.ContentType,
                OriginalFileName = fileRecord.OriginalFileName,
            };

            return dto;
        }

        public async Task<FileInfoDto> GetFileInfo(int fileId)
        {
            var fileRecord = await _db.FileRecords
                .Include(x => x.UploadedByUser)
                .FirstOrDefaultAsync(x => x.Id == fileId);

            var dto = new FileInfoDto(fileRecord!);

            return dto;
        }

        public async Task UploadFile(IFormFile file, int uploadedByUserId)
        {
            var uploadedByUser = await _db.Users.FindAsync(uploadedByUserId);

            var fileRecord = _fileRecordFactory.Create(file, file.FileName, uploadedByUser!);

            var stream = file.OpenReadStream();

            await _fileStorageProvider.UploadFileAsync(stream!, fileRecord.StorageFileName, fileRecord.ContentType);

            await _db.AddAsync(fileRecord);

            await _db.SaveChangesAsync();
        }
    }
}
