namespace Desyco.Dms.Domain.Students;

public class StudentStatusEntity : EntityBase<int>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;
    
    public string? TranslationKey { get; set; }
}
