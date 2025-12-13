using Desyco.Dms.Domain.Grades;
using Desyco.Dms.Domain.GradingScales;
using Desyco.Dms.Domain.Subjects;

namespace Desyco.Dms.Infrastructure.Subjects;

public class SubjectConfiguration : IEntityTypeConfiguration<SubjectEntity>
{
    public void Configure(EntityTypeBuilder<SubjectEntity> builder)
    {
        builder.ToTable("Subject");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);

        builder.HasOne<GradeEntity>().WithMany().HasForeignKey(x => x.GradeId);
        builder.HasOne<GradingScaleEntity>().WithMany().HasForeignKey(x => x.GradingScaleId);
        builder.HasOne<SubjectAreaEntity>().WithMany().HasForeignKey(x => x.SubjectAreaId);
    }
}
