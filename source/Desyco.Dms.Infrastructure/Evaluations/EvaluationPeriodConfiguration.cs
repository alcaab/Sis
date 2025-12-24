using Desyco.Dms.Domain.AcademicYears;
using Desyco.Dms.Domain.Evaluations;

namespace Desyco.Dms.Infrastructure.Evaluations;

public class EvaluationPeriodConfiguration : IEntityTypeConfiguration<EvaluationPeriodEntity>
{
    public void Configure(EntityTypeBuilder<EvaluationPeriodEntity> builder)
    {
        builder.ToTable("EvaluationPeriod");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.ShortName).HasMaxLength(25);
        builder.Property(x => x.Weight).HasEvaluationValuePrecision();

        builder.HasOne<AcademicYearEntity>().WithMany().HasForeignKey(x => x.AcademicYearId);
    }
}
