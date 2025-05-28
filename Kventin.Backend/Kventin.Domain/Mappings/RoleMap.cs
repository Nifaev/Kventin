using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired();

            builder.HasData(
                [
                    new() { Id = 1L, Name = "Student", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 2L, Name = "Teacher", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 3L, Name = "Parent", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 4L, Name = "SuperUser", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 5L, Name = "AdminSchedule", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 6L, Name = "AdminGroups", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 7L, Name = "AdminBase", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 8L, Name = "AdminAnnouncements", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 9L, Name = "AdminFinances", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 10L, Name = "AdminPersonalAccounts", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 11L, Name = "AdminRegistration", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 12L, Name = "AdminLessons", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                ]);
        }
    }
}
