using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Guardians;
using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Infrastructure.Common;

namespace Desyco.Dms.Infrastructure.Payments;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("Payment");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Number).HasMaxLength(20);
        builder.Property(x => x.TotalAmount).HasMoneyValuePrecision();

        builder.HasOne<GuardianEntity>().WithMany().HasForeignKey(x => x.GuardianId);
    }
}
