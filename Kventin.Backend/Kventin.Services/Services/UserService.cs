using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Filters;
using Kventin.Services.Dtos.User;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Kventin.Services.Interfaces.Tools;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class UserService(KventinContext db) : IUserService
    {
        private readonly KventinContext _db = db;
        
        public async Task<List<UserRoleDto>> GetAllRoles()
        {
            var roles = await _db.Roles
                .Select(x => new UserRoleDto { RoleName = x.Name })
                .ToListAsync();

            return roles;
        }

        public async Task SetUserRoles(int userId, List<UserRoleDto> dtos)
        {
            var rolenames = dtos
                .Select(x => x.RoleName)
                .ToList();

            var user = await GetUserWithRolesByIdAsync(userId) 
                ?? throw new EntityNotFoundException("Пользователь с таким Id не найден");

            if (!user.IsSuperUser && rolenames.Contains("SuperUser"))
                throw new NoAccessException("У вас недостаточно прав, чтобы присвоить роль SuperUser");

            var userRolenames = user.Roles
                .Select(x => x.Name)
                .ToList();

            var roles = await _db.Roles
                .Where(x => rolenames.Contains(x.Name) &&
                            !userRolenames.Contains(x.Name))
                .ToListAsync();

            if (roles.Count == 0)
                return;

            user.Roles.AddRange(roles);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteUserRole(int userId, List<UserRoleDto> dtos)
        {
            var rolenames = dtos
                .Select(x => x.RoleName)
                .ToList();

            var user = await GetUserWithRolesByIdAsync(userId)
                ?? throw new EntityNotFoundException("Пользователь с таким Id не найден");

            if (!user.IsSuperUser && rolenames.Contains("SuperUser"))
                throw new NoAccessException("У вас недостаточно прав, чтобы удалить роль SuperUser");

            user.Roles.RemoveAll(x => rolenames.Contains(x.Name));

            await _db.SaveChangesAsync();
        }

        public async Task<List<UserRoleDto>> GetUserRoles(int userId)
        {
            var user = await GetUserWithRolesByIdAsync(userId)
                ?? throw new EntityNotFoundException("Пользователь с таким Id не найден");

            var userRoles = user.Roles
                .Select(x => new UserRoleDto { RoleName = x.Name })
                .ToList();

            return userRoles;
        }

        public async Task<List<UsersRolesInfoDto>> GetUsersWithRoles(BaseFilterDto filter, int userId)
        {
            var isSuperUser = await _db.Users
                .Where(x => x.Id == userId)
                .Select(x => x.IsSuperUser)
                .FirstOrDefaultAsync();

            var query = _db.Users.Where(x => x.Id != userId);

            if (!isSuperUser)
                query = query.Where(x => !x.IsSuperUser);

            var dtos = await query
                .OrderBy(x => x.Id)
                .Skip(filter.Skip)
                .Take(filter.Take)
                .Select(x => new UsersRolesInfoDto
                {
                    UserId = x.Id,
                    Roles = x.Roles.Select(x => x.Name).ToList()
                })
                .ToListAsync();

            return dtos;
        }

        private async Task<User?> GetUserWithRolesByIdAsync(int userId)
        {
            return await _db.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
