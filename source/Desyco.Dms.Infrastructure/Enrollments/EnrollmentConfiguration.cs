using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.AcademicYears;
using Desyco.Dms.Domain.Enrollments;
using Desyco.Dms.Domain.Sections;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Enrollments;

public class EnrollmentConfiguration : IEntityTypeConfiguration<EnrollmentEntity>
{
    public void Configure(EntityTypeBuilder<EnrollmentEntity> builder)
    {
        builder.ToTable("Enrollment");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.WithdrawalReason).HasMaxLength(500);

        builder.HasOne<AcademicYearEntity>().WithMany().HasForeignKey(x => x.AcademicYearId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<SectionEntity>().WithMany().HasForeignKey(x => x.SectionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<EnrollmentStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.Status) // Property is 'Status' in EnrollmentEntity
            .OnDelete(DeleteBehavior.Restrict);
    }
}
