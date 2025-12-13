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

        builder.HasOne<AcademicYearEntity>().WithMany().HasForeignKey(x => x.AcademicYearId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<GradeEntity>().WithMany().HasForeignKey(x => x.GradeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ShiftEntity>().WithMany().HasForeignKey(x => x.ShiftId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ClassroomEntity>().WithMany().HasForeignKey(x => x.ClassroomId).OnDelete(DeleteBehavior.Restrict);
    }
}
