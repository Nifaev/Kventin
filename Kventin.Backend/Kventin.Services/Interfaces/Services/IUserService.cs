using Kventin.Services.Dtos.Users;

namespace Kventin.Services.Interfaces.Services
{
    public interface IUserService
    {
        public Task<List<UserShortInfoDto>> GetUsersChildren(long parentId);
        public Task SetChildrenForParent(long parentId, List<long> childrenIds);
        public Task<List<UserShortInfoDto>> GetAllUsersShortInfoByRole(string rolename);
    }
}
