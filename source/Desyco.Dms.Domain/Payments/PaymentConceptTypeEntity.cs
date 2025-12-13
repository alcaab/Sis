// For EntityBase

// For ITranslationKey

namespace Desyco.Dms.Domain.Payments;

public class PaymentConceptTypeEntity : EntityBase<PaymentConceptType>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;

    public string? TranslationKey { get; set; }
}
