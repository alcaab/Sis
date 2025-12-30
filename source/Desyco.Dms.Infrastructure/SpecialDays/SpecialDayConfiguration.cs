using Desyco.Dms.Domain.Evaluations;
using Desyco.Dms.Domain.SpecialDays;

namespace Desyco.Dms.Infrastructure.SpecialDays;

public class SpecialDayConfiguration : IEntityTypeConfiguration<SpecialDayEntity>
{
    public void Configure(EntityTypeBuilder<SpecialDayEntity> builder)
    {
        builder.ToTable("SpecialDay");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(e => e.Date).IsUnique();

        builder.Property(e => e.Name)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.EvaluationPeriodId)
            .IsRequired();
        
        builder.HasOne<EvaluationPeriodEntity>()
            .WithMany()
            .HasForeignKey(x => x.EvaluationPeriodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
