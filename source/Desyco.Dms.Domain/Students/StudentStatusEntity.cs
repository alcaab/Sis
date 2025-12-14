namespace Desyco.Dms.Domain.Students;

public class StudentStatusEntity : EntityBase<StudentStatus>
{
    public string Name { get; set; } = string.Empty;
    
    public string? TranslationKey { get; set; }
}
