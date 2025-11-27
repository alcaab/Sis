using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Shifts;

namespace Desyco.Dms.Infrastructure.Shifts;

public class ShiftConfiguration : IEntityTypeConfiguration<ShiftEntity>
{
    public void Configure(EntityTypeBuilder<ShiftEntity> builder)
    {
        builder.ToTable("Shift");
        builder.HasKey(x => x.Id);
    }
}
