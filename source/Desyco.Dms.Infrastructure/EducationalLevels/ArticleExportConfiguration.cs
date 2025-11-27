using Desyco.Dms.Domain.EducationalLevels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Dms.Infrastructure.EducationalLevels;

public class EducationLevelConfiguration : IEntityTypeConfiguration<EducationalLevelEntity>
{
    public void Configure(EntityTypeBuilder<EducationalLevelEntity> builder)
    {
        builder.ToTable("EducationLevel");
        
    }
}
