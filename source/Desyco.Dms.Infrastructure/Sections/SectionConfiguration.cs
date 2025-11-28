using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.AcademicYears;
using Desyco.Dms.Domain.Classrooms;
using Desyco.Dms.Domain.Grades;
using Desyco.Dms.Domain.Sections;
using Desyco.Dms.Domain.Shifts;

namespace Desyco.Dms.Infrastructure.Sections;

public class SectionConfiguration : IEntityTypeConfiguration<SectionEntity>
{
    public void Configure(EntityTypeBuilder<SectionEntity> builder)
    {
        builder.ToTable("Section");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);

        builder.HasOne<AcademicYearEntity>().WithMany().HasForeignKey(x => x.AcademicYearId);
        builder.HasOne<GradeEntity>().WithMany().HasForeignKey(x => x.GradeId);
        builder.HasOne<ShiftEntity>().WithMany().HasForeignKey(x => x.ShiftId);
        builder.HasOne<ClassroomEntity>().WithMany().HasForeignKey(x => x.ClassroomId);
    }
}
