using Desyco.Shared.Contracts.Interfaces; // Para ITranslationKey

namespace Desyco.Iam.Infrastructure.Persistence.Entities;

public class Feature : ITranslationKey
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!; // Ej: "AcademicYears"

    // La descripción de la Feature es ahora también su TranslationKey
    public string Description { get; set; } = null!; 
    string? ITranslationKey.TranslationKey { get => Description; set => Description = value ?? string.Empty; }

    public string? Group { get; set; } // Ej: "Configuración Académica"

    public int Order { get; set; }

    /// <summary>
    /// Comma-separated list of available custom permissions (e.g., "Approve,Export,Print").
    /// </summary>
    public string? CustomPermissions { get; set; }

    // Navigation properties for claims if needed, not directly here.
}
