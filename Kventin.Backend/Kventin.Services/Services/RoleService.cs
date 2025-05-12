using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class RoleService(KventinContext db) : IRoleService
    {
        private readonly KventinContext _db = db;

        public async Task<List<string>> GetAllRoles(List<string> authorizedUserRolenames)
        {
            var roles = new List<string>();

            if (authorizedUserRolenames.Contains("SuperUser"))
                roles = await _db.Roles
                    .Select(x => x.Name)
                    .ToListAsync();
            else
                roles = await _db.Roles
                    .Where(x => !x.Name.Contains("Admin") &&
                                x.Name != "SuperUser")
                    .Select(x => x.Name)
                    .ToListAsync();

            return roles;
        }

        public async Task SetUserRoles(int authorizedUserId, int userId, List<string> rolenamesToAdd)
        {
            var authorizedUser = await GetUserWithRolesByIdAsync(authorizedUserId);

            if (!authorizedUser!.IsSuperUser && authorizedUserId == userId)
                throw new NoAccessException("Вы не можете менять свои роли");

            if (!authorizedUser!.IsSuperUser && (rolenamesToAdd.Contains("SuperUser") || rolenamesToAdd.Any(x => x.Contains("Admin"))))
                throw new NoAccessException("У вас недостаточно прав, чтобы назначить данные роли");

            var user = await GetUserWithRolesByIdAsync(userId)
                ?? throw new EntityNotFoundException("Пользователь с таким Id не найден");

            var userRolenames = user.Roles
                .Select(x => x.Name)
                .ToList();

            var rolesToAdd = await _db.Roles
                .Where(x => rolenamesToAdd.Contains(x.Name) &&
                            !userRolenames.Contains(x.Name))
                .ToListAsync();

            if (rolesToAdd.Count == 0)
                return;

            user.Roles.AddRange(rolesToAdd);

            if (rolesToAdd.Any(x => x.Name == "SuperUser"))
                user.IsSuperUser = true;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteUserRole(int authorizedUserId, int userId, List<string> rolenamesToDelete)
        {
            var authorizedUser = await GetUserWithRolesByIdAsync(authorizedUserId);

            if (!authorizedUser!.IsSuperUser && authorizedUserId == userId)
                throw new NoAccessException("Вы не можете менять свои роли");

            if (!authorizedUser!.IsSuperUser && (rolenamesToDelete.Contains("SuperUser") || rolenamesToDelete.Any(x => x.Contains("Admin"))))
                throw new NoAccessException("У вас недостаточно прав, чтобы удалить данные роли");

            var user = await GetUserWithRolesByIdAsync(userId)
                ?? throw new EntityNotFoundException("Пользователь с таким Id не найден");

            user.Roles.RemoveAll(x => rolenamesToDelete.Contains(x.Name));

            if (rolenamesToDelete.Contains("SuperUser"))
                user.IsSuperUser = false;

            await _db.SaveChangesAsync();
        }

        public async Task<List<string>> GetUserRoles(int userId)
        {
            var user = await GetUserWithRolesByIdAsync(userId)
                ?? throw new EntityNotFoundException("Пользователь с таким Id не найден");

            var userRoles = user.Roles
                .Select(x => x.Name)
                .ToList();

            return userRoles;
        }

        public async Task<List<UserRoleInfoDto>> GetAllUsersWithRoles(int authorizedUserId)
        {
            var result = new List<UserRoleInfoDto>();

            var userRoles = await _db.Users
                .Where(x => x.Id == authorizedUserId)
                .SelectMany(x => x.Roles)
                .ToListAsync();

            var query = _db.Users
                .Include(x => x.Roles)
                .Where(x => x.Id != authorizedUserId);

            if (userRoles.All(x => x.Name != "SuperUser"))
                query = query.Where(x => x.Roles
                    .All(y => !y.Name.Contains("Admin") &&
                               y.Name != "SuperUser"));

            var take = 50;
            var usersCount = await query.CountAsync();
            var pageCount = Math.Ceiling(usersCount / (double)take);

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var users = await query
                    .Skip(pageNumber * take)
                    .Take(take)
                    .ToListAsync();

                var dtos = users
                    .Select(x => new UserRoleInfoDto
                    {
                        User = new UserShortInfoDto(x),
                        Roles = x.Roles
                            .Select(y => y.Name)
                            .ToList()
                    })
                    .ToList();

                result.AddRange(dtos);
            }

            return result;
        }

        private async Task<User?> GetUserWithRolesByIdAsync(int userId)
        {
            return await _db.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
