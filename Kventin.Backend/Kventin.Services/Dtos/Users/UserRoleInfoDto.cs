namespace Kventin.Services.Dtos.Users
{
    public class UserRoleInfoDto
    {
        public required UserShortInfoDto User {  get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
