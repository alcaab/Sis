namespace Desyco.Dms.Domain.Invoices;

public class InvoiceEntity : EntityBase<int>
{
    public int GuardianId { get; set; }

    public string Number { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public DateOnly DueDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TotalDiscount { get; set; }
    
    public decimal TotalAmount { get; set; }

    public InvoiceStatus Status { get; set; }
}
