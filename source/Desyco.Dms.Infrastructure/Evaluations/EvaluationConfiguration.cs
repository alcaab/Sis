using Desyco.Dms.Domain.Evaluations;

namespace Desyco.Dms.Infrastructure.Evaluations;

public class EvaluationConfiguration : IEntityTypeConfiguration<EvaluationEntity>
{
    public void Configure(EntityTypeBuilder<EvaluationEntity> builder)
    {
        builder.ToTable("Evaluation");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Weight).HasEvaluationValuePrecision();
    }
}
