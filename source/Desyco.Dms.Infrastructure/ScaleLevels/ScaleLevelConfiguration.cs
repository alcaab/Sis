using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.GradingScales;
using Desyco.Dms.Domain.ScaleLevels;

namespace Desyco.Dms.Infrastructure.ScaleLevels;

public class ScaleLevelConfiguration : IEntityTypeConfiguration<ScaleLevelEntity>
{
    public void Configure(EntityTypeBuilder<ScaleLevelEntity> builder)
    {
        builder.ToTable("ScaleLevel");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.ShortCode).HasMaxLength(10);

        builder.HasOne<GradingScaleEntity>().WithMany().HasForeignKey(x => x.GradingScaleId);
    }
}
