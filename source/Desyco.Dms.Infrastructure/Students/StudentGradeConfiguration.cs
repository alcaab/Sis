using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Domain.ScaleLevels;
using Desyco.Dms.Domain.Students;
using Desyco.Dms.Infrastructure.Common;

namespace Desyco.Dms.Infrastructure.Students;

public class StudentGradeConfiguration : IEntityTypeConfiguration<StudentGradeEntity>
{
    public void Configure(EntityTypeBuilder<StudentGradeEntity> builder)
    {
        builder.ToTable("StudentGrade");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Comments).HasMaxLength(500);
        builder.Property(x => x.Score).HasEvaluationValuePrecision();

        builder.HasOne<EvaluationEntity>().WithMany().HasForeignKey(x => x.EvaluationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ScaleLevelEntity>().WithMany().HasForeignKey(x => x.ScaleLevelId).OnDelete(DeleteBehavior.Restrict);
    }
}
