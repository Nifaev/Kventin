using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class TuitionTariffMap : IEntityTypeConfiguration<TuitionTariff>
    {
        public void Configure(EntityTypeBuilder<TuitionTariff> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
