using Desyco.Dms.Domain.EducationalLevels;

namespace Desyco.Dms.Infrastructure.EducationalLevels;

public class EducationalLevelConfiguration : IEntityTypeConfiguration<EducationalLevelEntity>
{
    public void Configure(EntityTypeBuilder<EducationalLevelEntity> builder)
    {
        builder.ToTable("EducationalLevel");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);

        
        builder.HasOne<EducationalLevelTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.LevelTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
