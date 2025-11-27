using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Classrooms;

namespace Desyco.Dms.Infrastructure.Classrooms;

public class ClassroomConfiguration : IEntityTypeConfiguration<ClassroomEntity>
{
    public void Configure(EntityTypeBuilder<ClassroomEntity> builder)
    {
        builder.ToTable("Classroom");
        builder.HasKey(x => x.Id);

        builder.HasOne<ClassroomTypeEntity>().WithMany().HasForeignKey(x => x.ClassroomTypeId);
    }
}
