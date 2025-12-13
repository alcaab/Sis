using Desyco.Shared.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Shared.Infrastructure.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<LanguageEntity>
{
    public void Configure(EntityTypeBuilder<LanguageEntity> builder)
    {
        builder.ToTable("Language", "dls");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Code)
            .HasMaxLength(10)
            .IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();
        
        // Seed initial data
        builder.HasData(
            new LanguageEntity { Id = 1, Name = "English", Code = "en", IsActive = true },
            new LanguageEntity { Id = 2, Name = "Español", Code = "es", IsActive = true }
        );
    }
}
