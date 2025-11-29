using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.EducationalLevels;

namespace Desyco.Dms.Infrastructure.EducationalLevels;

public class EducationalLevelTypeConfiguration : IEntityTypeConfiguration<EducationalLevelTypeEntity>
{
    public void Configure(EntityTypeBuilder<EducationalLevelTypeEntity> builder)
    {
        builder.ToTable("EducationalLevelType");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}
