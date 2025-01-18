using Kventin.Services.Dtos.Auth;
using Kventin.Services.Dtos.User;

namespace Kventin.Services.Interfaces.Services
{
    public interface IAuthService
    {
        public Task Register(RegisterDto dto);
        public Task<string> Login(LoginDto dto);
        public UserIdDto GetUserIdByToken(string token);
    }
}
