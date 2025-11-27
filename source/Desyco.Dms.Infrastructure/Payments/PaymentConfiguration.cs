using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Payments;

namespace Desyco.Dms.Infrastructure.Payments;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("Payment");
        builder.HasKey(x => x.Id);
    }
}
