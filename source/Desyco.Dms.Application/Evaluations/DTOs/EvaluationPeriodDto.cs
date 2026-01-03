namespace Desyco.Dms.Application.Evaluations.DTOs;

public record EvaluationPeriodDto
{
    public int Id { get; init; }
    
    public int AcademicYearId { get; init; }
    
    public int LevelTypeId { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string ShortName { get; init; } = string.Empty;
    
    public DateOnly StartDate { get; init; }
    
    public DateOnly EndDate { get; init; }
    
    public int? Sequence { get; init; }
    
    public decimal Weight { get; init; }
}
