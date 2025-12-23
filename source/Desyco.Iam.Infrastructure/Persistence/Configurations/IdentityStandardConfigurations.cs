using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Iam.Infrastructure.Persistence.Configurations;

public class IdentityStandardConfigurations : 
    IEntityTypeConfiguration<ApplicationUserRole>,
    IEntityTypeConfiguration<ApplicationUserLogin>,
    IEntityTypeConfiguration<ApplicationUserToken>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasIndex(ur => ur.RoleId);
    }

    public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
    {
        builder.ToTable("UserLogins");
        
        builder.Property(p => p.LoginProvider).HasMaxLength(128);
        builder.HasIndex(ul => ul.UserId);
    }

    public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
    {
        builder.ToTable("UserTokens");
    }
}
