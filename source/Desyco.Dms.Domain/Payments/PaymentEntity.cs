namespace Desyco.Dms.Domain.Payments;

public class PaymentEntity : EntityBase<int>
{
    public int GuardianId { get; set; }
    
    public string Number { get; set; } = string.Empty;
    
    public DateTime Date { get; set; }

    public decimal TotalAmount { get; set; }
}
