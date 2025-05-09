using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Interfaces.Tools
{
    public interface IJwtProvider
    {
        public string GenerateToken(int userId, string userLogin, List<string> rolenames, int selectedChildId = 0);

        public UserIdDto GetUserIdByToken(string token);

        public List<UserRoleDto> GetUserRolesByToken(string token);

        public string GetUserLoginByToken(string token);

        public UserIdDto GetChildIdByToken(string token);
    }
}
