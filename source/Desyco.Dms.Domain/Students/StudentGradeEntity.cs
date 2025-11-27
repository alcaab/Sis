namespace Desyco.Dms.Domain.Students;

public class StudentGradeEntity: EntityBase<int>
{
    public int EvaluationId { get; set; }
    
    public int StudentId { get; set; }

    public int? ScaleLevelId { get; set; }
    
    public decimal Score { get; set; }

    public string? Comments { get; set; }
}
