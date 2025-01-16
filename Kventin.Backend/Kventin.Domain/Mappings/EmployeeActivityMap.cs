using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class EmployeeActivityMap : IEntityTypeConfiguration<EmployeeActivity>
    {
        public void Configure(EntityTypeBuilder<EmployeeActivity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.EmployeeActivities)
                .IsRequired();
        }
    }
}
