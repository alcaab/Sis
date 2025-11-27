using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Guardians;

namespace Desyco.Dms.Infrastructure.Guardians;

public class GuardianConfiguration : IEntityTypeConfiguration<GuardianEntity>
{
    public void Configure(EntityTypeBuilder<GuardianEntity> builder)
    {
        builder.ToTable("Guardian");
        builder.HasKey(x => x.Id);
    }
}
