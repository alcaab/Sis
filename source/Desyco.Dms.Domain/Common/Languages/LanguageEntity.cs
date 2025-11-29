namespace Desyco.Dms.Domain.Common.Languages;

public class LanguageEntity : EntityBase<int>
{
    public string Name { get; set; } = string.Empty;
    
    public string Code { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
}
