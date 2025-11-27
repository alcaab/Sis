using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Invoices.FeeConcepts;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Invoices;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItemEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceItemEntity> builder)
    {
        builder.ToTable("InvoiceItem");
        builder.HasKey(x => x.Id);

        builder.HasOne<InvoiceEntity>().WithMany().HasForeignKey(x => x.InvoiceId);
        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId);
        builder.HasOne<FeeConceptEntity>().WithMany().HasForeignKey(x => x.FeeConceptId);
    }
}
