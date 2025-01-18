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
                    new() { Id = 1, Name = "Student", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 2, Name = "Teacher", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 3, Name = "Parent", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 4, Name = "SuperUser", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 5, Name = "AdminSchedule", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 6, Name = "AdminGroups", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 7, Name = "AdminStudyProgress", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 8, Name = "AdminAnnouncements", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 9, Name = "AdminFinances", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 10, Name = "AdminPersonalAccounts", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                    new() { Id = 11, Name = "AdminRegistration", CreateDateTime = new DateTime(2025, 1, 18, 19, 30, 00)},
                ]);
        }
    }
}
