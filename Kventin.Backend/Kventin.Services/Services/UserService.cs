using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
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
        
        public async Task SetUserRole(int userId, UserRoleDto dto)
        {
            var role = await _db.Roles
                .FirstOrDefaultAsync(x => x.Name.CompareTo(dto.RoleName) == 0) 
                ?? throw new EntityNotFoundException("Указанная роль не найдена");

            var user = await GetUserWithRolesByIdAsync(userId);

            if (user.Roles.Any(x => x.Name.CompareTo(role.Name) == 0))
                return;

            user.Roles.Add(role);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteUserRole(int userId, UserRoleDto dto)
        {
            var user = await GetUserWithRolesByIdAsync(userId);

            var roleToDelete = user.Roles
                .FirstOrDefault(x => x.Name.CompareTo(dto.RoleName) == 0);

            if (roleToDelete == null)
                return;

            user.Roles.Remove(roleToDelete);
            await _db.SaveChangesAsync();
        }

        public async Task<List<UserRoleDto>> GetUserRoles(int userId)
        {
            var user = await GetUserWithRolesByIdAsync(userId);

            var userRoles = user.Roles
                .Select(x => new UserRoleDto { RoleName = x.Name })
                .ToList();

            return userRoles;
        }

        private async Task<User> GetUserWithRolesByIdAsync(int userId)
        {
            return await _db.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new EntityNotFoundException("Пользователь с таким Id не найден");
        }
    }
}
