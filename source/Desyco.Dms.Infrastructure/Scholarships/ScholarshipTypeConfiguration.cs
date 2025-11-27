using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Scholarships;

namespace Desyco.Dms.Infrastructure.Scholarships;

public class ScholarshipTypeConfiguration : IEntityTypeConfiguration<ScholarshipTypeEntity>
{
    public void Configure(EntityTypeBuilder<ScholarshipTypeEntity> builder)
    {
        builder.ToTable("ScholarshipType");
        builder.HasKey(x => x.Id);
    }
}
