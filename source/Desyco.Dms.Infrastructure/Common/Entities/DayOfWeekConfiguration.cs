using Desyco.Dms.Domain.Common.Entities;

namespace Desyco.Dms.Infrastructure.Common.Entities;

public class DayOfWeekConfiguration : IEntityTypeConfiguration<DayOfWeekEntity>
{
    public void Configure(EntityTypeBuilder<DayOfWeekEntity> builder)
    {
        builder.ToTable("DayOfWeek");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.TranslationKey).HasMaxLength(100);
    }
}
