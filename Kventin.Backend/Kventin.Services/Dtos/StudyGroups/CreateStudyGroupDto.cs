namespace Kventin.Services.Dtos.StudyGroups
{
    public class CreateStudyGroupDto
    {
        public required string GroupName { get; set; }
        public required int SubjectId { get; set; }
        public required int TeacherId { get; set; }
    }
}
