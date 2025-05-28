using Kventin.Services.Dtos.Users;
using Microsoft.AspNetCore.Http;

namespace Kventin.Services.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<UserAccountInfoDto> GetUserAccountInfo(long userId);
        public Task UpdateUserAccountInfo(IRequestCookieCollection cookies, UpdateUserAccountInfoDto dto, long userId, long authorizedUserId);
    }
}
