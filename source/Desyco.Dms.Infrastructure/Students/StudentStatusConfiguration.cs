using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Students;

public class StudentStatusConfiguration : IEntityTypeConfiguration<StudentStatusEntity>
{
    public void Configure(EntityTypeBuilder<StudentStatusEntity> builder)
    {
        builder.ToTable("StudentStatus");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.TranslationKey).HasMaxLength(100);
    }
}
