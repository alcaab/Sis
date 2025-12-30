using Desyco.Dms.Domain.SpecialDays;

namespace Desyco.Dms.Infrastructure.SpecialDays;

public class RecurrentHolidayConfiguration : IEntityTypeConfiguration<RecurrentHolidayEntity>
{
    public void Configure(EntityTypeBuilder<RecurrentHolidayEntity> builder)
    {
        builder.ToTable("RecurrentHoliday");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(e => e.Date).IsUnique();

        builder.Property(e => e.Name)
            .HasMaxLength(30);

        builder.Property(e => e.Description)
            .HasMaxLength(255);
    }
}
