using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Kventin.DataAccess
{
    public class KventinContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudyGroup> StudyGroups { get; set; }
        public DbSet<User> Users { get; set; } 
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseAnswer> ExerciseAnswers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<EmployeeActivity> EmployeeActivities { get; set; }
        public DbSet<StudentActivity> StudentActivities { get; set; }
        public DbSet<EmployeeRate> EmployeeRates { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public DbSet<TuitionPayment> TuitionPayments { get; set; }
        public DbSet<TuitionTariff> TuitionTariffs { get; set; }

        public KventinContext(DbContextOptions<KventinContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SubjectMap());
            modelBuilder.ApplyConfiguration(new StudyGroupMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new LessonMap());
            modelBuilder.ApplyConfiguration(new MarkMap());
            modelBuilder.ApplyConfiguration(new ExerciseMap());
            modelBuilder.ApplyConfiguration(new ExerciseAnswerMap());
            modelBuilder.ApplyConfiguration(new MessageMap());
            modelBuilder.ApplyConfiguration(new NotificationMap());
            modelBuilder.ApplyConfiguration(new AnnouncementMap());
            modelBuilder.ApplyConfiguration(new EmployeeActivityMap());
            modelBuilder.ApplyConfiguration(new StudentActivityMap());
            modelBuilder.ApplyConfiguration(new EmployeeRateMap());
            modelBuilder.ApplyConfiguration(new EmployeeSalaryMap());
            modelBuilder.ApplyConfiguration(new TuitionPaymentMap());
            modelBuilder.ApplyConfiguration(new TuitionTariffMap());
        }
    }
}
