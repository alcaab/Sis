using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Students;

public class StudentConfiguration : IEntityTypeConfiguration<StudentEntity>
{
    public void Configure(EntityTypeBuilder<StudentEntity> builder)
    {
        builder.ToTable("Student");
        builder.HasKey(x => x.Id);

        builder.HasOne<StudentStatusEntity>().WithMany().HasForeignKey(x => x.Status);
    }
}
