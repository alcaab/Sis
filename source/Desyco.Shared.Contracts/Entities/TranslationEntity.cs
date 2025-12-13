namespace Desyco.Shared.Contracts.Entities;

public class TranslationEntity
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public int LanguageId { get; set; }
    public string Value { get; set; } = string.Empty;

    public LanguageEntity? Language { get; set; } // Propiedad de navegación
}
