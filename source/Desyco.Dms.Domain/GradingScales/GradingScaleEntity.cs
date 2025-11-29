namespace Desyco.Dms.Domain.GradingScales;

public class GradingScaleEntity : EntityBase<int>
{
    public string Name { get; set; } = string.Empty;

    public GradingScaleType TypeId { get; set; }
}
