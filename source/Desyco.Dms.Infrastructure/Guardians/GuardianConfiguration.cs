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
        
        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
        builder.Property(x => x.Address).HasMaxLength(255);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.Phone).HasMaxLength(20);
    }
}
