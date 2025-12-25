namespace Desyco.Dms.Application.Evaluations.DTOs;

public class EvaluationPeriodViewDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Weight { get; set; }
    public int? Sequence { get; set; }
}
