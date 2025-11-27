namespace Desyco.Dms.Domain.Evaluations;

public class EvaluationEntity : EntityBase<int>
{
    public int PeriodTypeId { get; set; }
    
    public int AssignmentId { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public decimal Weight { get; set; }
}
