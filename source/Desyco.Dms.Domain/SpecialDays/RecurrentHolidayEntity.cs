namespace Desyco.Dms.Domain.SpecialDays;

public class RecurrentHolidayEntity: EntityBase<int>
{
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateOnly Date { get; set; }
    
    public bool IsMandatory { get; set; }
}
