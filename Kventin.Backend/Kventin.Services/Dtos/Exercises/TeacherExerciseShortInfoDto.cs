using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Exercises
{
    public class TeacherExerciseShortInfoDto
    {
        [Required]
        public required int ExerciseId { get; set; }

        public DateTime? Deadline { get; set; }

        [Required]
        public required string Content { get; set; }

        [Required]
        public bool IsIndividual { get; set; }

        /// <summary>
        /// Если занятие индивидуальное, то будет не пустым
        /// </summary>
        public int? IndividualStudentId { get; set; }
        public string? IndividualStudentFullName { get; set; }
        public string? IndividualStudentShortName { get; set; }
    }
}
