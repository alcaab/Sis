using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Iam.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Email).HasMaxLength(256);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
        builder.Property(u => u.UserName).HasMaxLength(256);
        builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);
        builder.Property(u => u.SecurityStamp).HasMaxLength(128);
        builder.Property(u => u.ConcurrencyStamp).HasMaxLength(128);
        builder.Property(u => u.PhoneNumber).HasMaxLength(32);

        // Indexes are typically handled by base Identity configuration, 
        // but can be reiterated here if needed for exact match with snapshot.
        // The snapshot shows standard Identity indexes.
    }
}
