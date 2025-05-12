using Kventin.DataAccess;
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
    }
}
