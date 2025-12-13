

// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.EducationalLevels;

public class EducationalLevelTypeEntity : EntityBase<EducationalLevelType>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;
    
    public string? TranslationKey { get; set; }
}
