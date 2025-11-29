using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Domain.Students;
using Desyco.Dms.Infrastructure.Common;

namespace Desyco.Dms.Infrastructure.Payments;

public class PaymentDetailConfiguration : IEntityTypeConfiguration<PaymentDetailEntity>
{
    public void Configure(EntityTypeBuilder<PaymentDetailEntity> builder)
    {
        builder.ToTable("PaymentDetail");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Description).HasMaxLength(255);
        builder.Property(x => x.Amount).HasMoneyValuePrecision();

        builder.HasOne<PaymentEntity>().WithMany().HasForeignKey(x => x.PaymentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<InvoiceDetailEntity>().WithMany().HasForeignKey(x => x.InvoiceDetailId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<PaymentConceptTypeEntity>().WithMany().HasForeignKey(x => x.ConceptTypeId).OnDelete(DeleteBehavior.Restrict);
    }
}
