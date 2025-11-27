using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Teachers;

namespace Desyco.Dms.Infrastructure.Teachers;

public class TeacherConfiguration : IEntityTypeConfiguration<TeacherEntity>
{
    public void Configure(EntityTypeBuilder<TeacherEntity> builder)
    {
        builder.ToTable("Teacher");
        builder.HasKey(x => x.Id);
    }
}
