using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Iam.Infrastructure.Persistence.Configurations;

public class ApplicationUserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
    {
        builder.ToTable("UserClaims");

        builder.Property(uc => uc.Description).HasMaxLength(256);
        
        builder.Property(rc => rc.ClaimType).HasMaxLength(64);
        builder.Property(rc => rc.ClaimValue).HasMaxLength(100);
        
        builder.HasIndex(p => new {p.UserId, p.ClaimType, p.ClaimValue }).IsUnique();
        
        builder.Property(uc => uc.PermissionAction)
            .HasConversion<int>();
    }
}
