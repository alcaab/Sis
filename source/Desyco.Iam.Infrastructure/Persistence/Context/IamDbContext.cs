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
        public DbSet<Feature> Features { get; set; }
        // public DbSet<RoleFeature> RoleFeatures { get; set; } // Nuevo DbSet - REMOVED

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.HasDefaultSchema("dls");
            
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("Roles");
                b.Property(r => r.Description).HasMaxLength(256); // Configurar longitud
            });
            
            // builder.Entity<RoleFeature>(b => // RoleFeature configuration REMOVED
            // {
            //     b.ToTable("RoleFeatures");
            //     b.HasKey(rf => new { rf.RoleId, rf.FeatureId }); // Clave compuesta
            // });
            
            builder.Entity<RefreshToken>(b =>
            {
                b.ToTable("RefreshTokens");
                b.HasKey(rt => rt.Id); // PK
                b.Property(rt => rt.Token).IsRequired().HasMaxLength(200); // Token string
                b.Property(rt => rt.JwtId).IsRequired().HasMaxLength(50); // Jti
                b.Property(rt => rt.CreationDate).IsRequired();
                b.Property(rt => rt.ExpiryDate).IsRequired();
                b.Property(rt => rt.Used).IsRequired();
                b.Property(rt => rt.Invalidated).IsRequired();

                // Relación con ApplicationUser
                b.HasOne(rt => rt.User)
                 .WithMany() // No necesitamos una navegación en ApplicationUser para esto
                 .HasForeignKey(rt => rt.UserId)
                 .OnDelete(DeleteBehavior.Cascade); // Si el usuario se elimina, también sus refresh tokens
            });
            
            builder.Entity<Feature>(b =>
            {
                b.ToTable("Features");
                b.HasIndex(f => f.Code).IsUnique(); // Código de Feature debe ser único
                b.Property(f => f.Description).HasMaxLength(256);
                b.Property(f => f.Group).HasMaxLength(100);
                b.Property(f => f.CustomPermissions).HasMaxLength(500); // Comma-separated list
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToTable("UserClaims");
                // FK removed to decouple Identity from Features
                b.Property(uc => uc.PermissionAction)
                    .HasConversion<int>() // Guardar Enum como int
                    .IsRequired(false);
                b.Property(uc => uc.Description).HasMaxLength(256);
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable("RoleClaims");
                // FK removed to decouple Identity from Features
                b.Property(rc => rc.PermissionAction)
                    .HasConversion<int>() // Guardar Enum como int
                    .IsRequired(false);
                b.Property(rc => rc.Description).HasMaxLength(256);
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
