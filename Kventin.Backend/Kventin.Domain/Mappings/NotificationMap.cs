using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class NotificationMap : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Reciever)
                    .WithMany(x => x.Notifications)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

            builder.Property(x => x.Content)
                    .HasMaxLength(500);
        }
    }
}
