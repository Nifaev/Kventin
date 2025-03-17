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

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Classroom)
                .HasMaxLength(10);

            builder.Property(x => x.Topic)
                .HasMaxLength(100);
        }
    }
}
