namespace Desyco.Dms.Domain.Payments;

public class PaymentMethodDetailEntity : EntityBase<int>
{
    public int PaymentId { get; set; }
    
    public PaymentMethod PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public string? ReferenceNumber { get; set; }
}
