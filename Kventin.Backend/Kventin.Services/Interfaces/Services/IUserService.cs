using Kventin.Services.Dtos.Filters;
using Kventin.Services.Dtos.User;

namespace Kventin.Services.Interfaces.Services
{
    public interface IUserService
    {
        
        public Task SetUserRole(int userId, UserRoleDto dto);
        public Task DeleteUserRole(int userId, UserRoleDto dto);
        public Task<List<UserRoleDto>> GetUserRoles(int userId);
        public Task<List<UsersRolesInfoDto>> GetUsersWithRoles(BaseFilterDto filter, int userId);
        public Task<List<UserRoleDto>> GetAllRoles();
    }
}
