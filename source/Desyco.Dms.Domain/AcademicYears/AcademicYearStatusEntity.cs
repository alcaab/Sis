// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.AcademicYears;

public class AcademicYearStatusEntity : EntityBase<AcademicYearStatus>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;

    public string? TranslationKey { get; set; }
}
