using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Информация о файле
    /// </summary>
    public class FileRecord : BaseEntity
    {
        /// <summary>
        /// Имя файла, указанное пользователем
        /// </summary>
        public required string OriginalFileName { get; set; }

        /// <summary>
        /// Имя файла в облачном хранилище
        /// </summary>
        public required string StorageFileName { get; set; }

        /// <summary>
        /// MIME-тип файла
        /// </summary>
        public required string ContentType { get; set; }

        /// <summary>
        /// Размер файла
        /// </summary>
        public long FileSize { get; set; } 

        /// <summary>
        /// Id пользователя, загрузившего файл
        /// </summary>
        public int UploadedByUserId { get; set; }

        /// <summary>
        /// Пользователь, загрузивший файл
        /// </summary>
        public required User UploadedByUser { get; set; }

        /// <summary>
        /// Id занятия, к которому прикреплен файл
        /// </summary>
        public int? LessonId { get; set; }
        
        /// <summary>
        /// Занятие, к которому прикреплен файл
        /// </summary>
        public Lesson? Lesson { get; set; }

        /// <summary>
        /// Id задания, к которому прикреплен файл
        /// </summary>
        public int? ExerciseId { get; set; }

        /// <summary>
        /// Задание, к которому прикреплен файл
        /// </summary>
        public Exercise? Exercise { get; set; }

        /// <summary>
        /// Id ответа на задание, к которому прикреплен файл
        /// </summary>
        public int? ExerciseAnswerId { get; set; }

        /// <summary>
        /// Ответ на задание, к которому прикреплен файл
        /// </summary>
        public ExerciseAnswer? ExerciseAnswer { get; set; }

        /// <summary>
        /// Id уведомления, к которому прикреплен файл
        /// </summary>
        public int? NotificationId { get; set; }

        /// <summary>
        /// Уведомление, к которому прикреплен файл
        /// </summary>
        public Notification? Notification { get; set; }

        /// <summary>
        /// Id сообщения, к которому прикреплен файл
        /// </summary>
        public int? MessageId { get; set; }

        /// <summary>
        /// Сообщение, к которому прикреплен файл
        /// </summary>
        public Message? Message { get; set; }

        /// <summary>
        /// Id объяления, к которому прикреплен файл
        /// </summary>
        public int? AnnouncementId { get; set; }

        /// <summary>
        /// Объявление, к которому прикреплен файл
        /// </summary>
        public Announcement? Announcement { get; set; }


    }
}
