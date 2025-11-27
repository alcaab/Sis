using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Invoices;

namespace Desyco.Dms.Infrastructure.Invoices;

public class InvoiceStatusConfiguration : IEntityTypeConfiguration<InvoiceStatusEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceStatusEntity> builder)
    {
        builder.ToTable("InvoiceStatus");
        builder.HasKey(x => x.Id);
    }
}
