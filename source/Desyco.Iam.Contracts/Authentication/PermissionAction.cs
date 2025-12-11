namespace Desyco.Iam.Contracts.Authentication;

public enum PermissionAction
{
    None = 0,
    Read = 1,
    Write = 2, // Includes Create and Update
    Delete = 3
}
