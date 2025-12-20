namespace Desyco.Iam.Contracts.Permissions;

public record PredefinedPermission<T>(T Value, bool Inherited = false)
{
   // public bool Inherited { get; init; }
}
