namespace Desyco.Dms.Domain.Subjects;

public class SubjectEntity : EntityBase<int>
{
    public int GradeId { get; set; }

    public int GradingScaleId { get; set; }

    public int SubjectAreaId { get; set; }
    
    public string Name { get; set; } = string.Empty;   
}
