using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.GradingScales;

namespace Desyco.Dms.Infrastructure.GradingScales;

public class GradingScaleTypeConfiguration : IEntityTypeConfiguration<GradingScaleTypeEntity>
{
    public void Configure(EntityTypeBuilder<GradingScaleTypeEntity> builder)
    {
        builder.ToTable("GradingScaleType");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.TranslationKey).HasMaxLength(100);
    }
}
