using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Files
{
    public class DownloadFileDto
    {
        [Required]
        public required MemoryStream File { get; set; }

        [Required]
        public required string ContentType { get; set; }

        [Required]
        public required string OriginalFileName { get; set; }
    }
}
