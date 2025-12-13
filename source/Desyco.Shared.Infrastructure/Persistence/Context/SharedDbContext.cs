using System.Reflection;
using Desyco.Shared.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Shared.Infrastructure.Persistence.Context;

public class SharedDbContext(DbContextOptions<SharedDbContext> options) : DbContext(options)
{
    public DbSet<LanguageEntity> Languages { get; set; } = null!;
    public DbSet<TranslationEntity> Translations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("dls"); // Usa el esquema por defecto para entidades compartidas

        // Aplica todas las configuraciones definidas en este ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
