namespace Desyco.Dms.Domain.Common.Translations;

public class TranslationEntity : EntityBase<int>
{
    public string Key { get; set; } = string.Empty;
    
    public int LanguageId { get; set; }
    
    public string Value { get; set; } = string.Empty;
}
