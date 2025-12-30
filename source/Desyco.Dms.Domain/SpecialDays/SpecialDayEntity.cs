namespace Desyco.Dms.Domain.SpecialDays;

public class SpecialDayEntity: EntityBase<int>
{
    public int EvaluationPeriodId { get; set; }
    
    public SpecialDayType Type { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateOnly Date { get; set; }
    
    public TimeOnly? OpenTime { get; set; }
    
    public TimeOnly? StartTime { get; set; }
    
    public TimeOnly? EndTime { get; set; }
    
    public TimeOnly? CloseTime { get; set; }
}
