using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.EducationalLevels;

namespace Desyco.Dms.Infrastructure.EducationalLevels;

public class EducationalLevelConfiguration : IEntityTypeConfiguration<EducationalLevelEntity>
{
    public void Configure(EntityTypeBuilder<EducationalLevelEntity> builder)
    {
        builder.ToTable("EducationalLevel");
        builder.HasKey(x => x.Id);
    }
}
