namespace Kventin.Services.Dtos.Users
{
    public class UsersRolesInfoDto
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
