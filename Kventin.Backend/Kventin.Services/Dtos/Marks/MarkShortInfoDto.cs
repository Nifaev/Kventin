using Kventin.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Marks
{
    public class MarkShortInfoDto
    {
        [Required]
        public MarkValue MarkValue { get; set; }

        public string? TeacherComment { get; set; }
    }
}
