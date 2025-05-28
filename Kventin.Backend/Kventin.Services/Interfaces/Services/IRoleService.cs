using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Interfaces.Services
{
    public interface IRoleService
    {
        public Task SetUserRoles(long authorizedUserId, long userId, List<string> rolenamesToAdd);
        public Task DeleteUserRole(long authorizedUserId, long userId, List<string> rolenamesToDelete);
        public Task<List<string>> GetUserRoles(long userId);
        public Task<List<UserRoleInfoDto>> GetAllUsersWithRoles(long authorizedUserId);
        public Task<List<string>> GetAllRoles(List<string> authorizedUserRolenames);
    }
}
