using Kventin.DataAccess.Domain;
using Kventin.Services.Infrastructure.Extensions;

namespace Kventin.Services.Dtos.Users
{
    public class UserShortInfoDto
    {
        public UserShortInfoDto(User user)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            MiddleName = user.MiddleName;
            FullName = user.GetFullName();
            ShortName = user.GetShortName();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }
}
