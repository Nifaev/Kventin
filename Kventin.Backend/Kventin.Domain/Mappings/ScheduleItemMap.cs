using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class ScheduleItemMap : IEntityTypeConfiguration<ScheduleItem>
    {
        public void Configure(EntityTypeBuilder<ScheduleItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.ScheduleItems)
                .IsRequired();

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.ScheduleItems)
                .IsRequired();

            builder.HasOne(x => x.StudyGroup)
                .WithMany(x => x.ScheduleItems)
                .IsRequired();

            builder.HasOne(x => x.Schedule)
                .WithMany(x => x.Items)
                .IsRequired();
        }
    }
}
