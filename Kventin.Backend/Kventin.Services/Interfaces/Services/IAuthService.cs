using Kventin.Services.Dtos.Auth;
using Kventin.Services.Dtos.Users;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IAuthService
    {
        public Task Register(RegisterDto dto);
        public Task<string> Login(LoginDto dto);
        public long GetUserIdByCookie(IRequestCookieCollection cookie);
        public List<string> GetUserRolesByCookie(IRequestCookieCollection cookie);
        public Task<string> GetNewCookieWithChildId(IRequestCookieCollection cookie, long parentId, long childId);
        public long GetChildIdByCookie(IRequestCookieCollection cookie);
    }
}
