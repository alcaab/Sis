using Desyco.Shared.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Shared.Infrastructure.Persistence.Configurations;

public class TranslationConfiguration : IEntityTypeConfiguration<TranslationEntity>
{
    public void Configure(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.ToTable("Translation", "dls");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(x => x.LanguageId).IsRequired();
        
        builder.HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => new { x.Key, x.LanguageId }).IsUnique();
    }
}
