using Kventin.Services.Dtos.Filters;
using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Interfaces.Services
{
    public interface IUserService
    {
        public Task SetUserRoles(int userId, List<UserRoleDto> dtos);
        public Task DeleteUserRole(int userId, List<UserRoleDto> dtos);
        public Task<List<UserRoleDto>> GetUserRoles(int userId);
        public Task<List<UsersRolesInfoDto>> GetUsersWithRoles(BaseFilterDto filter, int userId);
        public Task<List<UserRoleDto>> GetAllRoles();
        public Task<GetUsersChildrenDto> GetUsersChildren(int parentId);
        public Task SetChildrenForParent(int parentId, List<int> childrenIds);
        public Task<List<UserShortInfoDto>> GetAllStudentsShortInfo();
    }
}
