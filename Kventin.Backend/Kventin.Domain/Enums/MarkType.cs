using System.ComponentModel;

namespace Kventin.DataAccess.Enums
{
    public enum MarkType
    {
        [Description("Оценка за занятие")]
        ForLesson = 0,

        [Description("Оценка за задание")]
        ForExercise = 1,
    }
}
