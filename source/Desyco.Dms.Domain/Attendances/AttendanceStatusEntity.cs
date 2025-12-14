// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.Attendances;

public class AttendanceStatusEntity : EntityBase<AttendanceStatus>
{
    public string Name { get; set; } = string.Empty;
}
