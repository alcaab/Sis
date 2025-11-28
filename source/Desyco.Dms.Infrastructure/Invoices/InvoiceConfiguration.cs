using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        builder.HasOne<GuardianEntity>().WithMany().HasForeignKey(x => x.GuardianId);
        builder.HasOne<InvoiceStatusEntity>().WithMany().HasForeignKey(x => x.Status);
    }
}
