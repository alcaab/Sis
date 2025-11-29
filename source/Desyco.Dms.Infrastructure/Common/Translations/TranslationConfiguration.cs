using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Common.Languages; 
using Desyco.Dms.Domain.Common.Translations;

namespace Desyco.Dms.Infrastructure.Common.Translations;

public class TranslationConfiguration : IEntityTypeConfiguration<TranslationEntity>
{
    public void Configure(EntityTypeBuilder<TranslationEntity> builder)
    {
        builder.ToTable("Translation");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Value)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(x => x.LanguageId).IsRequired();
        
        builder.HasOne<LanguageEntity>()
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(x => new { x.Key, x.LanguageId }).IsUnique();
    }
}
