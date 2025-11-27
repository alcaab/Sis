using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Domain.ScaleLevels;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Students;

public class StudentGradeConfiguration : IEntityTypeConfiguration<StudentGradeEntity>
{
    public void Configure(EntityTypeBuilder<StudentGradeEntity> builder)
    {
        builder.ToTable("StudentGrade");
        builder.HasKey(x => x.Id);

        builder.HasOne<EvaluationEntity>().WithMany().HasForeignKey(x => x.EvaluationId);
        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId);
        builder.HasOne<ScaleLevelEntity>().WithMany().HasForeignKey(x => x.ScaleLevelId);
    }
}
