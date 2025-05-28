using Kventin.Services.Dtos.Exercises;
using Kventin.Services.Dtos.Files;
using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Lessons
{
    public class LessonFullInfoDto
    {
        [Required]
        public required UserShortInfoDto Teacher { get; set; }

        [Required]
        public required long SubjectId { get; set; }

        [Required]
        public required string SubjectName { get; set; }

        [Required]
        public required long LessonId { get; set; }

        [Required]
        public required DateOnly Date { get; set; }

        [Required]
        public required TimeOnly StartTime { get; set; }

        [Required]
        public required TimeOnly EndTime { get; set; }

        public string? Classroom { get; set; }

        [Required]
        public required bool IsOnline { get; set; }

        public string? Topic { get; set; }

        public string? Description { get; set; }

        [Required]
        public required string LessonStatus { get; set; }

        [Required]
        public required string GroupName { get; set; }

        [Required]
        public required long GroupId { get; set; }

        public List<LessonStudentInfoDto> Students { get; set; } = [];

        public List<FileInfoDto> Files { get; set; } = [];

        public List<ExerciseShortInfoDto> Exercises { get; set; } = [];
    }
}
