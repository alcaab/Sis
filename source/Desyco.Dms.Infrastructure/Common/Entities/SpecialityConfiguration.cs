using Desyco.Dms.Domain.Common.Entities;

namespace Desyco.Dms.Infrastructure.Common.Entities;

public class SpecialityConfiguration : IEntityTypeConfiguration<SpecialityEntity>
{
    public void Configure(EntityTypeBuilder<SpecialityEntity> builder)
    {
        builder.ToTable("Speciality");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);
    }
}
