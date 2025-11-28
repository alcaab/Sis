using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Invoices.FeeConcepts;
using Desyco.Dms.Infrastructure.Common;

namespace Desyco.Dms.Infrastructure.Invoices.FeeConcepts;

public class FeeConceptConfiguration : IEntityTypeConfiguration<FeeConceptEntity>
{
    public void Configure(EntityTypeBuilder<FeeConceptEntity> builder)
    {
        builder.ToTable("FeeConcept");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Amount).HasMoneyValuePrecision();
    }
}
