namespace Kventin.Services.Dtos.User
{
    public class UsersRolesInfoDto
    {
        public int UserId { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
