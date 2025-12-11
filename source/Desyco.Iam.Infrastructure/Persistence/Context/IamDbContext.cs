using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Iam.Infrastructure.Persistence.Context;

    public class IamDbContext(DbContextOptions<IamDbContext> options) : IdentityDbContext<
        ApplicationUser, 
        ApplicationRole, 
        Guid, 
        ApplicationUserClaim, 
        ApplicationUserRole, 
        ApplicationUserLogin, 
        ApplicationRoleClaim, 
        ApplicationUserToken>(options)
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Definir esquema por defecto
            builder.HasDefaultSchema("dls");

            // Customizar nombres de tablas
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Roles");
            });
            
            builder.Entity<RefreshToken>(b =>
            {
                b.ToTable("RefreshTokens");
            });

            builder.Entity<ApplicationUserClaim>(b =>
        {
            b.ToTable("UserClaims");
        });

        builder.Entity<ApplicationRoleClaim>(b =>
        {
            b.ToTable("RoleClaims");
        });

        builder.Entity<ApplicationUserRole>(b =>
        {
            b.ToTable("UserRoles");
        });

        builder.Entity<ApplicationUserLogin>(b =>
        {
            b.ToTable("UserLogins");
        });

        builder.Entity<ApplicationUserToken>(b =>
        {
            b.ToTable("UserTokens");
        });
    }
}
