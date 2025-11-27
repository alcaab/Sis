using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Evaluations;

namespace Desyco.Dms.Infrastructure.Evaluations;

public class EvaluationConfiguration : IEntityTypeConfiguration<EvaluationEntity>
{
    public void Configure(EntityTypeBuilder<EvaluationEntity> builder)
    {
        builder.ToTable("Evaluation");
        builder.HasKey(x => x.Id);
    }
}
