namespace Desyco.Dms.Domain.Invoices;

public class InvoiceStatusEntity : EntityBase<InvoiceStatus>, ITranslationKey
{
    public string Name { get; set; } = string.Empty;
    
    public string? TranslationKey { get; set; }
}
