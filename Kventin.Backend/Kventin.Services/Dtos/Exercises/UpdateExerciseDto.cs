namespace Kventin.Services.Dtos.Exercises
{
    public class UpdateExerciseDto
    {
        public DateTime? DeadlineDateTime { get; set; }

        public string? Content { get; set; }

        public bool? IsIndividual { get; set; }

        public long? TeacherId { get; set; }

        public long? StudyGroupId { get; set; }

        public long? LessonId { get; set; }

        public long? IndividualStudentId { get; set; }
    }
}
