using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Interfaces.Services
{
    public interface IRoleService
    {
        public Task SetUserRoles(int authorizedUserId, int userId, List<string> rolenamesToAdd);
        public Task DeleteUserRole(int authorizedUserId, int userId, List<string> rolenamesToDelete);
        public Task<List<string>> GetUserRoles(int userId);
        public Task<List<UserRoleInfoDto>> GetAllUsersWithRoles(int authorizedUserId);
        public Task<List<string>> GetAllRoles(List<string> authorizedUserRolenames);
    }
}
