namespace Desyco.Dms.Domain.Invoices;

public class InvoiceDetailEntity : EntityBase<int>
{
    public int InvoiceId { get; set; }

    public int StudentId { get; set; }

    public int FeeConceptId { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public decimal NetAmount { get; set; }
    
    public decimal Payment { get; set; }

    public string Description  { get; set; } = string.Empty;

    public bool IsPaid { get; set; }
}
