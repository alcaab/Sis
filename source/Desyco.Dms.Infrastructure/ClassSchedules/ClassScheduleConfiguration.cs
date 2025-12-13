using Desyco.Dms.Domain.Classrooms;
using Desyco.Dms.Domain.ClassSchedules;
using Desyco.Dms.Domain.Common.Entities;

namespace Desyco.Dms.Infrastructure.ClassSchedules;

public class ClassScheduleConfiguration : IEntityTypeConfiguration<ClassScheduleEntity>
{
    public void Configure(EntityTypeBuilder<ClassScheduleEntity> builder)
    {
        builder.ToTable("ClassSchedule");
        builder.HasKey(x => x.Id);

        builder.HasOne<DayOfWeekEntity>().WithMany().HasForeignKey(x => x.DayOfWeekId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ClassroomEntity>().WithMany().HasForeignKey(x => x.ClassroomId).OnDelete(DeleteBehavior.Restrict);
    }
}
