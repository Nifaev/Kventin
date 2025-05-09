using Kventin.Services.Dtos.Auth;
using Kventin.Services.Dtos.Users;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IAuthService
    {
        public Task Register(RegisterDto dto);
        public Task<string> Login(LoginDto dto);
        public UserIdDto GetUserIdByCookie(IRequestCookieCollection cookie);
        public Task<string> GetNewCookieWithChildId(IRequestCookieCollection cookie, int parentId, int childId);
    }
}
