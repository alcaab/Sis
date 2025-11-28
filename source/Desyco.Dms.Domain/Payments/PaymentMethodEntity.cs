namespace Desyco.Dms.Domain.Payments;

public class PaymentMethodEntity : EntityBase<PaymentMethod>
{
    public string Name { get; set; } = string.Empty;
}
