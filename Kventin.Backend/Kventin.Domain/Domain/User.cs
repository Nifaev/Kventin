using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Имя
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public required string PhoneNumber { get; set; }
        
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Хэшированный пароль
        /// </summary>
        public required string HashedPassword { get; set; }

        /// <summary>
        /// Ссылка на ВКонтакте
        /// </summary>
        public string? VkLink { get; set; }

        /// <summary>
        /// Ссылка на Телеграм
        /// </summary>
        public string? TgLink { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public string? ContractNumber { get; set; }

        /// <summary>
        /// Является ли супер-пользователем
        /// </summary>
        public bool IsSuperUser { get; set; }

        /// <summary>
        /// Группы, в которых состоит ученик
        /// </summary>
        public List<StudyGroup> StudyGroups { get; set; } = [];

        /// <summary>
        /// Гррупы, к которым прикреплен преподаватель
        /// </summary>
        public List<StudyGroup> TeacherStudyGroups { get; set; } = [];

        /// <summary>
        /// Проведенные занятия (преподаватель)
        /// </summary>
        public List<Lesson> ConductedLessons { get; set; } = [];

        /// <summary>
        /// Занятия по расписанию, на которые назначен преподаватель
        /// </summary>
        public List<ScheduleItem> ScheduleItems { get; set; } = [];

        /// <summary>
        /// Посещенные занятия (ученик)
        /// </summary>
        public List<Lesson> AttendedLessons { get; set; } = [];

        /// <summary>
        /// Полученные оценки (ученик)
        /// </summary>
        public List<Mark> RecievedMarks { get; set; } = [];

        /// <summary>
        /// Выставленные оценки (преподаватель)
        /// </summary>
        public List<Mark> AssignedMarks { get; set; } = [];

        /// <summary>
        /// Выданные задания (преподаватель)
        /// </summary>
        public List<Exercise> AssignedExercises { get; set; } = [];

        /// <summary>
        /// Родители (ученик)
        /// </summary>
        public List<User> Parents { get; set; } = [];

        /// <summary>
        /// Дети (родитель)
        /// </summary>
        public List<User> Children { get; set; } = [];

        /// <summary>
        /// Ответы на задания (ученик)
        /// </summary>
        public List<ExerciseAnswer> ExerciseAnswers { get; set; } = [];

        /// <summary>
        /// Отправленные сообщения
        /// </summary>
        public List<Message> SentMessages { get; set; } = [];

        /// <summary>
        /// Полученные сообщения
        /// </summary>
        public List<Message> RecievedMessages { get; set; } = [];

        /// <summary>
        /// Уведомления
        /// </summary>
        public List<Notification> Notifications { get; set; } = [];

        /// <summary>
        /// Объявления (администратор)
        /// </summary>
        public List<Announcement> Announcements { get; set; } = [];

        /// <summary>
        /// Учет рабочих часов (преподаватель/администратор)
        /// </summary>
        public List<EmployeeActivity> EmployeeActivities { get; set; } = [];

        /// <summary>
        /// Учет учебных часов (ученик)
        /// </summary>
        public List<StudentActivity> StudentActivities { get; set; } = [];

        /// <summary>
        /// Ставка сотрудника
        /// </summary>
        public EmployeeRate? EmployeeRate { get; set; }

        /// <summary>
        /// Оплата занятий (ученик, родители)
        /// </summary>
        public List<TuitionPayment> TuitionPayment { get; set; } = [];

        /// <summary>
        /// Выплата зарплаты (преподаватель/администратор)
        /// </summary>
        public List<EmployeeSalary> EmployeeSalary { get; set; } = [];

        /// <summary>
        /// Тарифы, по которым обучается ученик 
        /// </summary>
        public List<TuitionTariff> TuitionTariffs { get; set; } = [];

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public List<Role> Roles { get; set; } = [];

        /// <summary>
        /// Индивидуальные задания
        /// </summary>
        public List<Exercise> IndividualExercises { get; set; } = [];

        /// <summary>
        /// Файлы, загруженные пользователем
        /// </summary>
        public List<FileRecord> UploadedFiles { get; set; } = [];
    }
}
