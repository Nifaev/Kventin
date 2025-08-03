using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Объявление
    /// </summary>
    public class Announcement : BaseEntity
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Содежрание
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// Id автора
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public required User Author { get; set; }


        /// <summary>
        /// Прикрепленные файлы
        /// </summary>
        public List<FileRecord> Files { get; set; } = [];
    }
}
