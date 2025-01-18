using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.User;

namespace Kventin.Services.Interfaces.Tools
{
    public interface IJwtProvider
    {
        public string GenerateToken(int userId, string userLogin, List<Role> roles);

        public UserIdDto GetUserIdByToken(string token);
    }
}
