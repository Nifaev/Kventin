using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.HasMany(x => x.StudyGroups)
                .WithMany(x => x.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentStudyGroups",
                    x => x.HasOne<StudyGroup>().WithMany().HasForeignKey("StudyGroupId").OnDelete(DeleteBehavior.Cascade),
                    x => x.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.Cascade));

            builder.HasMany(x => x.AttendedLessons)
                .WithMany(x => x.StudentsAttended)
                .UsingEntity<Dictionary<string, object>>(   
                    "StudentLessons",
                    x => x.HasOne<Lesson>().WithMany().HasForeignKey("LessonId").OnDelete(DeleteBehavior.NoAction),
                    x => x.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.NoAction));

            builder.HasMany(x => x.Parents)
                .WithMany(x => x.Children)
                .UsingEntity<Dictionary<string, object>>(
                    "ParentsChildren",
                    x => x.HasOne<User>().WithMany().HasForeignKey("ParentId").OnDelete(DeleteBehavior.NoAction),
                    x => x.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.NoAction));

            builder.HasMany(x => x.TuitionTariffs)
                .WithMany(x => x.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentsTariffs",
                    x => x.HasOne<TuitionTariff>().WithMany().HasForeignKey("TariffId").OnDelete(DeleteBehavior.NoAction),
                    x => x.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.NoAction));

            builder.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersRoles",
                    x => x.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.NoAction),
                    x => x.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.NoAction),
                    x => x.HasData([new { RoleId = 4, UserId = 1 }]));

            builder.HasData([
                new User
                {
                    Id = 1,
                    FirstName = "Суперпользователь",
                    LastName = "Встроенный",
                    PhoneNumber = "1234567890",
                    HashedPassword = "$2a$11$fOB6qIW/7qIQzJWq.mcS6ugc6UoFPAWctpSDZJQj5uaKTNiqiQ9xO",
                    IsSuperUser = true,
                    CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)
                }
            ]);
        }
    }
}
