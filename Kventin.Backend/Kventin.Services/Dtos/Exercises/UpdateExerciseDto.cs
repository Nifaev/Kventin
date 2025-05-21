namespace Kventin.Services.Dtos.Exercises
{
    public class UpdateExerciseDto
    {
        public DateTime? DeadlineDateTime { get; set; }

        public string? Content { get; set; }

        public bool? IsIndividual { get; set; }

        public int? TeacherId { get; set; }

        public int? StudyGroupId { get; set; }

        public int? LessonId { get; set; }

        public int? IndividualStudentId { get; set; }
    }
}
