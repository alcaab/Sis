using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Iam.Infrastructure.Persistence.Configurations;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("Roles");

        builder.Property(r => r.Description).HasMaxLength(256);
        
        builder.Property(r => r.Name).HasMaxLength(256);
        builder.Property(r => r.NormalizedName).HasMaxLength(256);
        
        builder.HasIndex(r => r.NormalizedName).IsUnique();
    }
}
