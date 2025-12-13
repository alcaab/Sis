using Desyco.Dms.Domain.Grades;

namespace Desyco.Dms.Infrastructure.Grades;

public class GradeConfiguration : IEntityTypeConfiguration<GradeEntity>
{
    public void Configure(EntityTypeBuilder<GradeEntity> builder)
    {
        builder.ToTable("Grade");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}
