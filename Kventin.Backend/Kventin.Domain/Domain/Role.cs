using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Пользовательская роль
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Название роли
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Пользователи, принадлежащие данной роли
        /// </summary>
        public List<User> Users { get; set; } = [];
    }
}
