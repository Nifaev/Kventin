using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Domain.Base;
using Kventin.DataAccess.Enums;
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

        public async Task DeleteFiles(List<int> fileIds)
        {
            if (fileIds.Count == 0) 
                return;

            var fileRecords = await _db.FileRecords
                .Where(x => fileIds.Contains(x.Id))
                .ToListAsync();

            if (fileRecords.Count == 0)
                return;

            var storageFileNames = fileRecords
                .Select(x => x.StorageFileName)
                .ToList();

            await _fileStorageProvider.DeleteFilesAsync(storageFileNames);

            _db.FileRecords.RemoveRange(fileRecords);

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

        public async Task UploadFiles<T>(List<IFormFile> files, int uploadedByUserId, FileLinkType fileLinkType, int linkedEntityId = 0) where T : BaseEntity
        {
            var uploadedByUser = await _db.Users.FindAsync(uploadedByUserId);

            if (fileLinkType != FileLinkType.None && linkedEntityId == 0)
                throw new ArgumentException("Не передано зачение параметра linkedEntityId");

            T? entity;

            if (fileLinkType != FileLinkType.None)
                entity = await _db.Set<T>().FindAsync(linkedEntityId)
                    ?? throw new ArgumentException($"Сущность {fileLinkType} с переданным Id не найдена");
            else
                entity = null;

            var filesInfo = files
                .Select(x => new
                {
                    File = x,
                    FileRecord = _fileRecordFactory.Create<T>(x, x.FileName, uploadedByUser!, fileLinkType, entity)
                })
                .ToList();

            var uploadFileDtos = filesInfo
                .Select(x => new UploadFileDto
                { 
                    ContentType = x.File.ContentType,
                    Stream = x.File.OpenReadStream(),
                    StorageFileName = x.FileRecord.StorageFileName,
                })
                .ToList();

            await _fileStorageProvider.UploadFilesAsync(uploadFileDtos);

            await _db.FileRecords.AddRangeAsync(filesInfo.Select(x => x.FileRecord));

            await _db.SaveChangesAsync();
        }

        public async Task UploadFileWithoutLinks(IFormFile file, int uploadedByUserId)
        {
            await UploadFiles<Lesson>([file], uploadedByUserId, FileLinkType.None);
        }
    }
}
