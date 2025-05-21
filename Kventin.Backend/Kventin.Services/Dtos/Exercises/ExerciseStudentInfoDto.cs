using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Marks;
using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Dtos.Exercises
{
    public class ExerciseStudentInfoDto(User user) : UserShortInfoDto(user)
    {
        public List<MarkInfoDto> Marks { get; set; } = [];
    }
}
