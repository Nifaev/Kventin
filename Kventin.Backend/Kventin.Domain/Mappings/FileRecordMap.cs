using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class FileRecordMap : IEntityTypeConfiguration<FileRecord>
    {
        public void Configure(EntityTypeBuilder<FileRecord> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.UploadedByUser)
                .WithMany(x => x.UploadedFiles)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.Announcement)
                .WithMany(x => x.Files);

            builder.HasOne(x => x.Exercise)
                .WithMany(x => x.Files);

            builder.HasOne(x => x.ExerciseAnswer)
                .WithMany(x => x.Files);

            builder.HasOne(x => x.Lesson)
                .WithMany(x => x.Files);

            builder.HasOne(x => x.Message)
                .WithMany(x => x.Files);

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.Files);
        }
    }
}
