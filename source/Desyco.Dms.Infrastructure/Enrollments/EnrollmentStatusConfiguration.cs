using Desyco.Dms.Domain.Enrollments;

namespace Desyco.Dms.Infrastructure.Enrollments;

public class EnrollmentStatusConfiguration : IEntityTypeConfiguration<EnrollmentStatusEntity>
{
    public void Configure(EntityTypeBuilder<EnrollmentStatusEntity> builder)
    {
        builder.ToTable("EnrollmentStatus");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}
