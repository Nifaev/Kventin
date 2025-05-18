using System.ComponentModel;

namespace Kventin.DataAccess.Enums
{
    /// <summary>
    /// С чем связан файл
    /// </summary>
    public enum FileLinkType
    {
        [Description("Файл для занятия")]
        Lesson = 0,

        [Description("Файл для задания")]
        Exercise = 1,

        [Description("Файл для ответа на задание")]
        ExerciseAnswer = 2,

        [Description("Файл для уведомления")]
        Notification = 3,

        [Description("Файл для сообщения")]
        Message = 4,

        [Description("Файл для объявления")]
        Announcement = 5,

        [Description("Ни с чем не связан")]
        None = 100,
    }
}
