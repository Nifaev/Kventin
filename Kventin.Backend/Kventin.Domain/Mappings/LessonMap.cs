using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class LessonMap : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.ConductedLessons)
                .IsRequired();

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Lessons)
                .IsRequired();

            builder.HasOne(x => x.ScheduleItem)
                .WithMany(x => x.Lessons);
        }
    }
}
