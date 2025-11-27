namespace Desyco.Dms.Domain.ScaleLevels;

public class ScaleLevelEntity: EntityBase<int>
{
    public int GradingScaleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? ShortCode { get; set; }

    public decimal NumericEquivalent { get; set; }
}
