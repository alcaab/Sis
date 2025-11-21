namespace Desyco.ContractManager.Application.Common.Auth;

public interface IAuthGuard<in T>
{
    PermissionCheckResult CheckPermissions(T instance, IUserContext user);
}
