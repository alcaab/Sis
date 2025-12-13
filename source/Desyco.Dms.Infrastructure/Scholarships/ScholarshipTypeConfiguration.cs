using Desyco.Dms.Domain.Scholarships;

namespace Desyco.Dms.Infrastructure.Scholarships;

public class ScholarshipTypeConfiguration : IEntityTypeConfiguration<ScholarshipTypeEntity>
{
    public void Configure(EntityTypeBuilder<ScholarshipTypeEntity> builder)
    {
        builder.ToTable("ScholarshipType");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Requirements).HasMaxLength(500);
        builder.Property(x => x.DiscountPercentage).HasPercentageValuePrecision();
    }
}
