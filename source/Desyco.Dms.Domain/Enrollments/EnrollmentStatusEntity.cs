// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.Enrollments;

public class EnrollmentStatusEntity : EntityBase<EnrollmentStatus>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;
    
    public string? TranslationKey { get; set; }
}
