// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.AcademicYears;

public class AcademicYearStatusEntity : EntityBase<AcademicYearStatus>
{
    public string Name { get; set; } = string.Empty;

    public string? TranslationKey { get; set; }
}
