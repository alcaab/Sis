using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Permissions;

namespace Desyco.Iam.Contracts.Interfaces;

public interface IPermissionService
{
    Task<List<FeatureDto>> GetAllFeaturesAsync();
    Task<List<RoleClaimDto>> GetRoleClaimsAsync(Guid roleId);
    Task UpdateRolePermissionsAsync(Guid roleId, List<FeaturePermission> updatedPermissions);
    Task<bool> HasPermissionAsync(string userId, string featureCode, PermissionAction action);
}

public class FeaturePermission
{
    public Guid FeatureId { get; set; }
    public string FeatureCode { get; set; } = null!;
    public PermissionAction Action { get; set; }
    public bool IsGranted { get; set; }
}
