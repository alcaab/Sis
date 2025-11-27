using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Attendances;
using Desyco.Dms.Domain.Enrollments;

namespace Desyco.Dms.Infrastructure.Attendances;

public class AttendanceConfiguration : IEntityTypeConfiguration<AttendanceEntity>
{
    public void Configure(EntityTypeBuilder<AttendanceEntity> builder)
    {
        builder.ToTable("Attendance");
        builder.HasKey(x => x.Id);

        builder.HasOne<EnrollmentEntity>().WithMany().HasForeignKey(x => x.EnrollmentId);
        builder.HasOne<AttendanceStatusEntity>().WithMany().HasForeignKey(x => x.Status);
    }
}
