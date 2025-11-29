namespace Desyco.Dms.Domain.Payments;

public class PaymentMethodEntity : EntityBase<PaymentMethod>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;

    public string? TranslationKey { get; set; }
}
