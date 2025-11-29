using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Payments;

namespace Desyco.Dms.Infrastructure.Payments;

public class PaymentConceptTypeConfiguration : IEntityTypeConfiguration<PaymentConceptTypeEntity>
{
    public void Configure(EntityTypeBuilder<PaymentConceptTypeEntity> builder)
    {
        builder.ToTable("PaymentConceptType");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.TranslationKey).HasMaxLength(100);
    }
}
