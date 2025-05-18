using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Sender)
                .WithMany(x => x.SentMessages)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
                
            builder.HasOne(x => x.Reciever)
                .WithMany(x => x.RecievedMessages)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
