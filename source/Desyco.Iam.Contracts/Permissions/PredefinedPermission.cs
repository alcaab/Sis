namespace Desyco.Iam.Contracts.Permissions;

public record PredefinedPermission(bool Value)
{
    public bool Inherited { get; set; }
    
   public string? Name { get; init; }
}
