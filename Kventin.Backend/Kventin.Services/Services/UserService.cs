using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.Services.Dtos.Users;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class UserService(KventinContext db) : IUserService
    {
        private readonly KventinContext _db = db;

        public async Task<List<UserShortInfoDto>> GetAllUsersShortInfoByRole(string rolename)
        {
            var studentsQuery = _db.Users
                .Where(x => x.Roles
                    .Select(y => y.Name)
                    .Contains(rolename));

            var take = 50;
            var studentsCount = await studentsQuery.CountAsync();
            var pageCount = Math.Ceiling(studentsCount / (double)take);
            var result = new List<UserShortInfoDto>();

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var students = await studentsQuery
                    .Skip(pageNumber * take)
                    .Take(take)
                    .ToListAsync();

                var dtos = students
                    .Select(x => new UserShortInfoDto(x))
                    .ToList();

                result.AddRange(dtos);
            }

            result = result
                .OrderBy(x => x.FullName)
                .ToList();

            return result;
        }

        public async Task SetChildrenForParent(int parentId, List<int> childrenIds)
        {
            var parent = await _db.Users
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Id == parentId &&
                                          x.Roles.Select(y => y.Name).Contains("Parent"));

            if (parent == null)
                throw new EntityNotFoundException("Родитель с таким Id не найден");

            var children = await _db.Users
                .Include(x => x.Parents)
                .Where(x => childrenIds.Contains(x.Id) &&
                            x.Roles.Select(y => y.Name).Contains("Student"))
                .ToListAsync();

            if (!children.Any())
                throw new EntityNotFoundException("Не найдено ни одного ученика по переданным Id");

            parent.Children.AddRange(children);

            await _db.SaveChangesAsync();
        }

        public async Task<List<UserShortInfoDto>> GetUsersChildren(int parentId)
        {
            var children = await _db.Users
                .Include(x => x.Children)
                .Where(x => x.Id == parentId)
                .SelectMany(x => x.Children)
                .ToListAsync();

            if (!children.Any())
                return new List<UserShortInfoDto>();

            var result = children
                .Select(x => new UserShortInfoDto(x))
                .ToList();

            return result;
        }

        public async Task<List<string>> GetAllRoles()
        {
            var roles = await _db.Roles
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
