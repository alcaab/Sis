using Desyco.Dms.Domain.Attendances;

namespace Desyco.Dms.Infrastructure.Attendances;

public class AttendanceStatusConfiguration : IEntityTypeConfiguration<AttendanceStatusEntity>
{
    public void Configure(EntityTypeBuilder<AttendanceStatusEntity> builder)
    {
        builder.ToTable("AttendanceStatus");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}
