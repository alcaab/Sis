using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Permissions;

namespace Desyco.Iam.Contracts.Interfaces;

public interface IPermissionService
{
    Task<List<FeatureDto>> GetAllFeaturesAsync();
    
    Task<List<RoleClaimDto>> GetRoleClaimsAsync(Guid roleId);
    
    Task UpdateRolePermissionsAsync(Guid roleId, List<FeaturePermission> updatedPermissions);
    
    Task<RolePermissionSchemaDto> GetRolePermissionSchemaAsync(Guid roleId);
    
    Task<bool> HasPermissionAsync(string userId, IEnumerable<string> userRoles, string featureCode, PermissionAction action);
}
