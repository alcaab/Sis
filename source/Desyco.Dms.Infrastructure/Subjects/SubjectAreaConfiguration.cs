using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Subjects;

namespace Desyco.Dms.Infrastructure.Subjects;

public class SubjectAreaConfiguration : IEntityTypeConfiguration<SubjectAreaEntity>
{
    public void Configure(EntityTypeBuilder<SubjectAreaEntity> builder)
    {
        builder.ToTable("SubjectArea");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(100);


    }
}
