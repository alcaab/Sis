namespace Desyco.Dms.Domain.Evaluations;

public class EvaluationPeriodEntity : EntityBase<int>
{
    public int AcademicYearId { get; set; }
    
    public int LevelTypeId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string ShortName { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }
    
    public int? Sequence { get; set; }

    public decimal Weight { get; set; }
}
