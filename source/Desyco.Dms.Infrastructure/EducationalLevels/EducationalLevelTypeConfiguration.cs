using Desyco.Dms.Domain.EducationalLevels;

namespace Desyco.Dms.Infrastructure.EducationalLevels;

public class EducationalLevelTypeConfiguration : IEntityTypeConfiguration<EducationalLevelTypeEntity>
{
    public void Configure(EntityTypeBuilder<EducationalLevelTypeEntity> builder)
    {
        builder.ToTable("EducationalLevelType");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.TranslationKey).HasMaxLength(100);
    }
}
