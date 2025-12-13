using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Domain.Teachers;

namespace Desyco.Dms.Infrastructure.Teachers;

public class TeacherSpecialtyConfiguration : IEntityTypeConfiguration<TeacherSpecialtyEntity>
{
    public void Configure(EntityTypeBuilder<TeacherSpecialtyEntity> builder)
    {
        builder.ToTable("TeacherSpecialty");
        builder.HasKey(x => new { x.TeacherId, x.SpecialtyId });

        builder.HasOne<TeacherEntity>().WithMany().HasForeignKey(x => x.TeacherId);
        builder.HasOne<SpecialityEntity>().WithMany().HasForeignKey(x => x.SpecialtyId);
    }
}
