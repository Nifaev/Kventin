using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kventin.DataAccess.Mappings
{
    public class MarkMap : IEntityTypeConfiguration<Mark>
    {
        public void Configure(EntityTypeBuilder<Mark> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.RecievedMarks)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.Exercise)
                .WithMany(x => x.Marks);

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.AssignedMarks)
                .IsRequired();

            builder.HasOne(x => x.Lesson)
                .WithMany(x => x.Marks);

            builder.Property(x => x.Comment)
                .HasMaxLength(500);
        }
    }
}
