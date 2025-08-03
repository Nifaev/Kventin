using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Files
{
    public class FileInfoDto
    {
        public FileInfoDto(FileRecord fileRecord) 
        {
            FileId = fileRecord.Id;
            ContentType = fileRecord.ContentType;
            OriginalFileName = fileRecord.OriginalFileName;
            FileSize = fileRecord.SizeToString();
            UploadDateTime = fileRecord.CreateDateTime;
            UploadedByUser = new UserShortInfoDto(fileRecord.UploadedByUser);
        }

        [Required]
        public int FileId { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public string OriginalFileName { get; set; }

        [Required]
        public string FileSize { get; set; }

        [Required]
        public UserShortInfoDto UploadedByUser { get; set; }

        [Required]
        public DateTime UploadDateTime { get; set; }
    }
}
