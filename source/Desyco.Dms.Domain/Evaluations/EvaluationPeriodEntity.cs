namespace Desyco.Dms.Domain.Evaluations;

public class EvaluationPeriodEntity : EntityBase<int>
{
    public int AcademicYearId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public decimal Weight { get; set; }
}
