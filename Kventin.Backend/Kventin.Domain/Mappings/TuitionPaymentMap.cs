using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class TuitionPaymentMap : IEntityTypeConfiguration<TuitionPayment>
    {
        public void Configure(EntityTypeBuilder<TuitionPayment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Payer)
                .WithMany(x => x.TuitionPayment)
                .IsRequired();
        }
    }
}
