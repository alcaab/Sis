namespace Desyco.Dms.Application.SpecialDays.DTOs;

public record SpecialDayEvaluationPeriodDto
{
    public int Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public DateOnly StartDate { get; init; }
    
    public DateOnly EndDate { get; init; }
    
    public int? Sequence { get; init; }
    
    public int NumberOfMonths
    {
        get => (EndDate.Year - StartDate.Year) * 12
               + (EndDate.Month - StartDate.Month)
               + 1;
    }

    public IReadOnlyCollection<SpecialDayDto> Days { get; init; } = [];
}

public record SpecialDayEducationalLevelDto
{
    public int Id { get; init; }
    
    public string? Name { get; init; }
    
    public IReadOnlyCollection<SpecialDayEvaluationPeriodDto> Periods { get; init; } = [];
}
