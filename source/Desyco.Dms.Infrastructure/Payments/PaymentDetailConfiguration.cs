using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Invoices;
using Desyco.Dms.Domain.Payments;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Payments;

public class PaymentDetailConfiguration : IEntityTypeConfiguration<PaymentDetailEntity>
{
    public void Configure(EntityTypeBuilder<PaymentDetailEntity> builder)
    {
        builder.ToTable("PaymentDetail");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Description).HasMaxLength(255);

        builder.HasOne<PaymentEntity>().WithMany().HasForeignKey(x => x.PaymentId);
        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId);
        builder.HasOne<InvoiceDetailEntity>().WithMany().HasForeignKey(x => x.InvoiceDetailId);
        builder.HasOne<PaymentConceptTypeEntity>().WithMany().HasForeignKey(x => x.ConceptTypeId);
    }
}
