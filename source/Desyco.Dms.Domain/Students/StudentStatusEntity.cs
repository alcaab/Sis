

// For EntityBase and ITranslationKey

namespace Desyco.Dms.Domain.Students;

public class StudentStatusEntity : EntityBase<StudentStatus>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;
    
    public string? TranslationKey { get; set; }
}
