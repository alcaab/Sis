using Desyco.Dms.Domain.Classrooms;

namespace Desyco.Dms.Infrastructure.Classrooms;

public class ClassroomTypeConfiguration : IEntityTypeConfiguration<ClassroomTypeEntity>
{
    public void Configure(EntityTypeBuilder<ClassroomTypeEntity> builder)
    {
        builder.ToTable("ClassroomType");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
    }
}
