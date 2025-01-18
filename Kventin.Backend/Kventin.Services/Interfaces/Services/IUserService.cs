using Kventin.Services.Dtos.User;

namespace Kventin.Services.Interfaces.Services
{
    public interface IUserService
    {
        
        public Task SetUserRole(int userId, UserRoleDto dto);
        public Task DeleteUserRole(int userId, UserRoleDto dto);
        public Task<List<UserRoleDto>> GetUserRoles(int userId);
    }
}
