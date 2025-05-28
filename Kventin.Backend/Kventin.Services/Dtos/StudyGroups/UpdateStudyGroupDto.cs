namespace Kventin.Services.Dtos.StudyGroups
{
    public class UpdateStudyGroupDto
    {
        public string? GroupName { get; set; }
        public long? SubjectId { get; set; }
        public long? TeacherId { get; set; }
    }
}
