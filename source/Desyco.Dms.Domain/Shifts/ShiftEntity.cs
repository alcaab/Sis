namespace Desyco.Dms.Domain.Shifts;

public class ShiftEntity : EntityBase<ShiftType>
{
    public string Name { get; set; } = string.Empty;
    
    public TimeOnly? StartTime { get; set; }
    
    public TimeOnly? EndTime { get; set; }
    
    public string? TranslationKey { get; set; }
}
