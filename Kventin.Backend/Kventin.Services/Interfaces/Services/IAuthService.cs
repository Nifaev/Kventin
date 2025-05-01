using Kventin.Services.Dtos.Auth;
using Kventin.Services.Dtos.Users;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IAuthService
    {
        public Task Register(RegisterDto dto);
        public Task<string> Login(LoginDto dto);
        UserIdDto GetUserIdByCookie(IRequestCookieCollection cookie);
    }
}
