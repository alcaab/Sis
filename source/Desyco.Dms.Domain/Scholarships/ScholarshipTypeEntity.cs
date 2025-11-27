namespace Desyco.Dms.Domain.Scholarships;

public class ScholarshipTypeEntity : EntityBase<int>
{
    public string Name { get; set; } = string.Empty;

    public decimal DiscountPercentage { get; set; }
    
    public string? Requirements  { get; set; }

    public bool IsActive { get; set; }
}
