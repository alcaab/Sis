using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Permissions;

namespace Desyco.Iam.Contracts.Interfaces;

public interface IPermissionService
{
    Task<List<FeatureDto>> GetAllFeaturesAsync(string languageCode);
    
    Task<List<RoleClaimDto>> GetRoleClaimsAsync(Guid roleId); // No translation needed here
    
    Task UpdateRolePermissionsAsync(Guid roleId, List<FeaturePermission> updatedPermissions);
    
    Task<PermissionSchemaDto> GetPermissionSchemaForRoleAsync(Guid roleId, string languageCode);

    Task<PermissionSchemaDto> GetPermissionSchemaForUserAsync(Guid userId, string languageCode);
    Task<bool> HasPermissionAsync(string userId, IEnumerable<string> userRoles, string featureCode, PermissionAction action);
}
