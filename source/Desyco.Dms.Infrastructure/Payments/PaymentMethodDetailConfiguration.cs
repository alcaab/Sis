using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Infrastructure.Common;

namespace Desyco.Dms.Infrastructure.Payments;

public class PaymentMethodDetailConfiguration : IEntityTypeConfiguration<PaymentMethodDetailEntity>
{
    public void Configure(EntityTypeBuilder<PaymentMethodDetailEntity> builder)
    {
        builder.ToTable("PaymentMethodDetail");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ReferenceNumber).HasMaxLength(50);
        builder.Property(x => x.Amount).HasMoneyValuePrecision();

        builder.HasOne<PaymentEntity>().WithMany().HasForeignKey(x => x.PaymentId);
        builder.HasOne<PaymentMethodEntity>().WithMany().HasForeignKey(x => x.PaymentMethodId);
    }
}
