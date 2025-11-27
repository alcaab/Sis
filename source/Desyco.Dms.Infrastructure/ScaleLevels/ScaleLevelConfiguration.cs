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

        builder.HasOne<GradingScaleEntity>().WithMany().HasForeignKey(x => x.GradingScaleId);
    }
}
