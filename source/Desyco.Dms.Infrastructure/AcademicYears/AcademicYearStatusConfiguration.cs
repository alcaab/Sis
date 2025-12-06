using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.AcademicYears;

namespace Desyco.Dms.Infrastructure.AcademicYears;

public class AcademicYearStatusConfiguration : IEntityTypeConfiguration<AcademicYearStatusEntity>
{
    public void Configure(EntityTypeBuilder<AcademicYearStatusEntity> builder)
    {
        builder.ToTable("AcademicYearStatus");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.TranslationKey).HasMaxLength(100);
    }
}
