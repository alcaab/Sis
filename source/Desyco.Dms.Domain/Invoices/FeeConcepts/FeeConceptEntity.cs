namespace Desyco.Dms.Domain.Invoices.FeeConcepts;

public class FeeConceptEntity : EntityBase<int>
{
    public string Name { get; set; } = string.Empty;

    public decimal Amount { get; set; }
}
