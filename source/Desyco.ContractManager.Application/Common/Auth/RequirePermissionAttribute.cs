namespace Desyco.ContractManager.Application.Common.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public class RequirePermissionAttribute : Attribute
{
    public RequirePermissionAttribute(params string[] permissions)
    {
        RequireAll = true;
        Permissions = permissions;
    }

    public RequirePermissionAttribute(bool requireAll, params string[] permissions)
    {
        RequireAll = requireAll;
        Permissions = permissions;
    }

    public bool RequireAll { get; }

    public string[] Permissions { get; }
}
