using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class ExerciseMap : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.AssignedExercises)
                .IsRequired();

            builder.HasOne(x => x.Lesson)
                .WithMany(x => x.Exercises)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.IndividualStudent)
                .WithMany(x => x.IndividualExercises);

            builder.HasOne(x => x.StudyGroup)
                .WithMany(x => x.RecievedExercises)
                .IsRequired();
        }
    }
}
