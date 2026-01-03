using Desyco.Dms.Domain.SpecialDays;

namespace Desyco.Dms.Application.SpecialDays.DTOs;

public record SpecialDayDto
{
    public int Id { get; init; }
    
    public int EvaluationPeriodId { get; init; }
    
    public SpecialDayType Type { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;
    
    public DateOnly Date { get; init; }
    
    public TimeOnly? OpenTime { get; init; }
    
    public TimeOnly? StartTime { get; init; }
    
    public TimeOnly? EndTime { get; init; }
    
    public TimeOnly? CloseTime { get; init; }
}
