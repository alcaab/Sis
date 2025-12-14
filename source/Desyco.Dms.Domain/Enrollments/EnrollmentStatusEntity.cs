// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.Enrollments;

public class EnrollmentStatusEntity : EntityBase<EnrollmentStatus>
{
    public string Name { get; set; } = string.Empty;
}
