using Desyco.Dms.Domain.Common.Base;

namespace Desyco.Dms.Domain.GradingScales;

public class GradingScaleTypeEntity : EntityBase<GradingScaleType>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;
    
    public string? TranslationKey { get; set; }
}
