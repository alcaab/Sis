using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Common.Entities;

namespace Desyco.Dms.Infrastructure.Common.Entities;

public class RelationshipConfiguration : IEntityTypeConfiguration<RelationshipEntity>
{
    public void Configure(EntityTypeBuilder<RelationshipEntity> builder)
    {
        builder.ToTable("Relationship");
        builder.HasKey(x => x.Id);
    }
}
