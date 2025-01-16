using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class ExerciseAnswerMap : IEntityTypeConfiguration<ExerciseAnswer>
    {
        public void Configure(EntityTypeBuilder<ExerciseAnswer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.ExerciseAnswers)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.Exercise)
                .WithMany(x => x.Answers);

            builder.Property(x => x.Content)
                .HasMaxLength(500);
        }
    }
}
