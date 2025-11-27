using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.AcademicYears;
using Desyco.Dms.Domain.Evaluations;

namespace Desyco.Dms.Infrastructure.Evaluations;

public class EvaluationPeriodConfiguration : IEntityTypeConfiguration<EvaluationPeriodEntity>
{
    public void Configure(EntityTypeBuilder<EvaluationPeriodEntity> builder)
    {
        builder.ToTable("EvaluationPeriod");
        builder.HasKey(x => x.Id);

        builder.HasOne<AcademicYearEntity>().WithMany().HasForeignKey(x => x.AcademicYearId);
    }
}
