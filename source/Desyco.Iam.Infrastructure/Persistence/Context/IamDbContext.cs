using Desyco.Iam.Infrastructure.Persistence.Entities;
using Desyco.Shared.Contracts.Entities;
using Desyco.Shared.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<TranslationEntity> Translations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.HasDefaultSchema("dls");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyConfigurationsFromAssembly(typeof(LanguageConfiguration).Assembly);
        }
    }
