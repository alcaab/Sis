using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Invoices.FeeConcepts;
using Desyco.Dms.Domain.Students;
using Desyco.Dms.Infrastructure.Common;

namespace Desyco.Dms.Infrastructure.Invoices;

public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetailEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceDetailEntity> builder)
    {
        builder.ToTable("InvoiceDetail");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Description).HasMaxLength(255);
        builder.Property(x => x.Amount).HasMoneyValuePrecision();
        builder.Property(x => x.Discount).HasMoneyValuePrecision();
        builder.Property(x => x.NetAmount).HasMoneyValuePrecision();
        builder.Property(x => x.Payment).HasMoneyValuePrecision();

        builder.HasOne<InvoiceEntity>().WithMany().HasForeignKey(x => x.InvoiceId);
        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId);
        builder.HasOne<FeeConceptEntity>().WithMany().HasForeignKey(x => x.FeeConceptId);
    }
}
