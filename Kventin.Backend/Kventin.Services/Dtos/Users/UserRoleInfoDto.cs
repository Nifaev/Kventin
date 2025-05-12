using System.ComponentModel.DataAnnotations;

namespace Kventin.Services.Dtos.Users
{
    public class UserRoleInfoDto
    {
        [Required]
        public required UserShortInfoDto User {  get; set; }

        public List<string> Roles { get; set; } = [];
    }
}
