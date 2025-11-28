namespace Desyco.Dms.Domain.Payments;

public class PaymentDetailEntity : EntityBase<int>
{
    public int PaymentId { get; set; }
    
    public int? StudentId { get; set; }

    public int? InvoiceDetailId { get; set; }
    
    public PaymentConceptType ConceptTypeId { get; set; }
    
    public string Description  { get; set; } = string.Empty;
    
    public DateTime Date { get; set; }
    
    public decimal Amount { get; set; }
}
