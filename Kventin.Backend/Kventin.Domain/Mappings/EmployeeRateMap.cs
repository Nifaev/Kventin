using Kventin.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kventin.DataAccess.Mappings
{
    public class EmployeeRateMap : IEntityTypeConfiguration<EmployeeRate>
    {
        public void Configure(EntityTypeBuilder<EmployeeRate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Employee)
                .WithOne(x => x.EmployeeRate)
                .IsRequired();
        }
    }
}
