using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.GradingScales;

namespace Desyco.Dms.Infrastructure.GradingScales;

public class GradingScaleConfiguration : IEntityTypeConfiguration<GradingScaleEntity>
{
    public void Configure(EntityTypeBuilder<GradingScaleEntity> builder)
    {
        builder.ToTable("GradingScale");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);

        builder.HasOne<GradingScaleTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.TypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
