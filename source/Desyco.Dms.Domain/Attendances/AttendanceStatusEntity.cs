// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.Attendances;

public class AttendanceStatusEntity : EntityBase<AttendanceStatus>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;

    public string? TranslationKey { get; set; }
}
