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

        // Snapshot says "int?", so we configure it as such if needed, 
        // usually enums are int by default in EF Core.
        builder.Property(uc => uc.PermissionAction)
            .HasConversion<int>();
    }
}
