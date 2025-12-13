using Desyco.Dms.Domain.Guardians;
using Desyco.Dms.Domain.Invoices;

namespace Desyco.Dms.Infrastructure.Invoices;

public class InvoiceConfiguration : IEntityTypeConfiguration<InvoiceEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
    {
        builder.ToTable("Invoice");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Number).HasMaxLength(20);
        builder.Property(x => x.SubTotal).HasMoneyValuePrecision();
        builder.Property(x => x.TotalDiscount).HasMoneyValuePrecision();
        builder.Property(x => x.TotalAmount).HasMoneyValuePrecision();

        builder.HasOne<GuardianEntity>().WithMany().HasForeignKey(x => x.GuardianId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<InvoiceStatusEntity>().WithMany().HasForeignKey(x => x.Status).OnDelete(DeleteBehavior.Restrict);
    }
}
