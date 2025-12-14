// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.GradingScales;

public class GradingScaleTypeEntity : EntityBase<GradingScaleType>
{
    public string Name { get; set; } = string.Empty;
}
