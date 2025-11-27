using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Attendances;

namespace Desyco.Dms.Infrastructure.Attendances;

public class AttendanceStatusConfiguration : IEntityTypeConfiguration<AttendanceStatusEntity>
{
    public void Configure(EntityTypeBuilder<AttendanceStatusEntity> builder)
    {
        builder.ToTable("AttendanceStatus");
        builder.HasKey(x => x.Id);
    }
}
