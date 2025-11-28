using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Enrollments;
using Desyco.Dms.Domain.Scholarships;

namespace Desyco.Dms.Infrastructure.Scholarships;

public class ScholarshipAwardConfiguration : IEntityTypeConfiguration<ScholarshipAwardEntity>
{
    public void Configure(EntityTypeBuilder<ScholarshipAwardEntity> builder)
    {
        builder.ToTable("ScholarshipAward");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Justification).HasMaxLength(500);

        builder.HasOne<EnrollmentEntity>().WithMany().HasForeignKey(x => x.EnrollmentId);
        builder.HasOne<ScholarshipTypeEntity>().WithMany().HasForeignKey(x => x.ScholarshipTypeId);
    }
}
