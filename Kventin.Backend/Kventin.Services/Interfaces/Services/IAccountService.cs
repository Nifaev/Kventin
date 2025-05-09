using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<UserAccountInfoDto> GetUserAccountInfo(int userId);
        public Task UpdateUserAccountInfo(UpdateUserAccountInfoDto dto, int userId, int authorizedUserId);
    }
}
