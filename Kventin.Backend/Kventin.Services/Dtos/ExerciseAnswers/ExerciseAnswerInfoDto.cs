using Kventin.Services.Dtos.Files;
using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.ExerciseAnswers
{
    public class ExerciseAnswerInfoDto
    {
        [Required]
        public required UserShortInfoDto Student { get; set; }

        [Required]
        public required long ExerciseId { get; set; }

        [Required]
        public required long LessonId { get; set; }

        [Required]
        public required long ExerciseAnswerId { get; set; }

        [Required]
        public required string Content { get; set; }

        public List<FileInfoDto> Files { get; set; } = [];
    }
}
