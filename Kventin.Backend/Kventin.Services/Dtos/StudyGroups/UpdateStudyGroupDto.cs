namespace Kventin.Services.Dtos.StudyGroups
{
    public class UpdateStudyGroupDto
    {
        public string? GroupName { get; set; }
        public int? SubjectId { get; set; }
        public int? TeacherId { get; set; }
    }
}
