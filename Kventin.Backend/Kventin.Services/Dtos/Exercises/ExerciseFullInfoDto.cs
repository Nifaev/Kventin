using Kventin.Services.Dtos.ExerciseAnswers;
using Kventin.Services.Dtos.Files;
using Kventin.Services.Dtos.Users;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Exercises
{
    public class ExerciseFullInfoDto
    {
        [Required]
        public required long ExerciseId { get; set; } 
        
        public DateTime? DeadlineDateTime { get; set; }

        [Required]
        public required DateTime CreateDateTime { get; set; }

        [Required]
        public required string Content { get; set; }

        [Required]
        public required bool IsIndividual { get; set; }

        [Required]
        public required long StudyGroupId { get; set; }

        [Required]
        public required string StudyGroupName { get; set; }

        [Required]
        public required long LessonId { get; set; }

        [Required]
        public required UserShortInfoDto Teacher { get; set; }

        public ExerciseStudentInfoDto? IndividualStudent { get; set; }

        public List<ExerciseAnswerInfoDto> Answers { get; set; } = [];

        public List<ExerciseStudentInfoDto> Students { get; set; } = [];

        public List<FileInfoDto> Files { get; set; } = [];
    }
}
