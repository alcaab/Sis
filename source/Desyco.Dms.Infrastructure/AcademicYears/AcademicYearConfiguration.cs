using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.AcademicYears;

namespace Desyco.Dms.Infrastructure.AcademicYears;

public class AcademicYearConfiguration : IEntityTypeConfiguration<AcademicYearEntity>
{
    public void Configure(EntityTypeBuilder<AcademicYearEntity> builder)
    {
        builder.ToTable("AcademicYear");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        
        builder.HasOne<AcademicYearStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.Status)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
