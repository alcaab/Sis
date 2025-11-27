namespace Desyco.Dms.Domain.Guardians;

public class GuardianEntity : EntityBase<int>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? Address  { get; set; }
    
    public string? Email { get; set; }
    
    public string? Phone { get; set; }

    public bool IsFinancialResponsible { get; set; }
}
