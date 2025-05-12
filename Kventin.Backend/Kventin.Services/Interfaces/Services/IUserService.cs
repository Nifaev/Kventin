using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Interfaces.Services
{
    public interface IUserService
    {
        public Task<List<UserShortInfoDto>> GetUsersChildren(int parentId);
        public Task SetChildrenForParent(int parentId, List<int> childrenIds);
        public Task<List<UserShortInfoDto>> GetAllUsersShortInfoByRole(string rolename);
    }
}
