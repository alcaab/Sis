using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Contracts.Permissions;
using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Iam.Infrastructure.Authentication.Permissions;

public class PermissionService(
    IamDbContext dbContext,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager) : IPermissionService
{
    public async Task UpdateRolePermissionsAsync(Guid roleId, List<FeaturePermission> updatedPermissions)
    {
        var role = await roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
        {
            throw new ArgumentException($"Role with ID {roleId} not found.");
        }

        // Get existing granular claims for the role
        var existingClaims = await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId && rc.FeatureId != null)
            .ToListAsync();

        // 1. Identify Claims to Remove
        // A claim should be removed if it is NOT present in the updatedPermissions list (considering IsGranted state).
        var claimsToRemove = new List<ApplicationRoleClaim>();
        
        foreach (var claim in existingClaims)
        {
            // Try to find a matching entry in the incoming list
            var match = updatedPermissions.FirstOrDefault(up => 
                up.FeatureId == claim.FeatureId &&
                (
                    // Match standard permission (Action matches)
                    (up.Action != PermissionAction.None && up.Action == claim.PermissionAction) ||
                    // Match custom permission (Action is None, match by name suffix in ClaimValue)
                    (up.Action == PermissionAction.None && claim.PermissionAction is null or PermissionAction.None &&
                     !string.IsNullOrEmpty(up.CustomActionName) &&
                     claim.ClaimValue!.EndsWith($".{up.CustomActionName}", StringComparison.OrdinalIgnoreCase))
                )
            );

            // If no match found in request, or found but explicitly revoked (IsGranted=false) -> Remove
            if (match is not { IsGranted: true })
            {
                claimsToRemove.Add(claim);
            }
        }
        
        if (claimsToRemove.Count != 0) 
        {
            dbContext.RoleClaims.RemoveRange(claimsToRemove);
        }

        // 2. Identify Permissions to Add
        foreach (var up in updatedPermissions.Where(u => u.IsGranted))
        {
            // Check if this permission already exists in the database
            var exists = existingClaims.Any(claim => 
                claim.FeatureId == up.FeatureId &&
                (
                    (up.Action != PermissionAction.None && up.Action == claim.PermissionAction) ||
                    (up.Action == PermissionAction.None && claim.PermissionAction is null or PermissionAction.None &&
                     !string.IsNullOrEmpty(up.CustomActionName) &&
                     claim.ClaimValue!.EndsWith($".{up.CustomActionName}", StringComparison.OrdinalIgnoreCase))
                )
            );

            // If it doesn't exist (and wasn't just marked for removal above - which it wouldn't be if it exists), add it.
            // Note: If it existed but was marked for removal, it means IsGranted=false, so we skip this loop anyway.
            if (!exists)
            {
                var actionName = up.Action != PermissionAction.None ? up.Action.ToString() : up.CustomActionName;
                
                await dbContext.RoleClaims.AddAsync(new ApplicationRoleClaim
                {
                    RoleId = roleId,
                    ClaimType = "Permission",
                    ClaimValue = $"Permissions.{up.FeatureCode}.{actionName}",
                    FeatureId = up.FeatureId,
                    PermissionAction = up.Action, // Will be None (0) for custom
                    Description = $"Claim for {up.FeatureCode} {actionName}"
                });
            }
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<PermissionSchemaDto> GetPermissionSchemaForRoleAsync(Guid roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
        {
            throw new ArgumentException($"Role with ID {roleId} not found.");
        }
        
        var allFeaturesDto = await (from f in dbContext.Features
                                    orderby f.Group, f.Order
                                    select new FeatureDto
                                    {
                                        Id = f.Id,
                                        Code = f.Code,
                                        Description =  f.Description,
                                        Group = f.Group,
                                        Order = f.Order,
                                        CustomPermissions = f.CustomPermissions
                                    }).ToListAsync();

        var claims = await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .Select(rc => new GenericClaimDto(rc.FeatureId, rc.PermissionAction, rc.ClaimValue))
            .ToListAsync();
        
        ArgumentNullException.ThrowIfNull(role.Name);
        
        return BuildPermissionSchema(roleId, role.Name, allFeaturesDto, claims);
    }
    
    public async Task<PermissionSchemaDto> GetPermissionSchemaForUserAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        
        ArgumentNullException.ThrowIfNull(user);

        var allFeaturesDto = await (from f in dbContext.Features
                                    orderby f.Group, f.Order
                                    select new FeatureDto
                                    {
                                        Id = f.Id,
                                        Code = f.Code,
                                        Description = f.Description,
                                        Group = f.Group,
                                        Order = f.Order,
                                        CustomPermissions = f.CustomPermissions
                                    }).ToListAsync();

        var claims = await dbContext.UserClaims
            .Where(uc => uc.UserId == userId)
            .Select(uc => new GenericClaimDto(uc.FeatureId, uc.PermissionAction, uc.ClaimValue))
            .ToListAsync();
        
        return BuildPermissionSchema(userId, GetComposedName(user), allFeaturesDto, claims);
    }

    private static PermissionSchemaDto BuildPermissionSchema(
        Guid entityId, 
        string entityName, 
        List<FeatureDto> features,
        List<GenericClaimDto> claims)
    {
        var claimsByFeature = claims
            .Where(c => c.FeatureId.HasValue)
            .ToLookup(c => c.FeatureId.Value);

        var permissionGroups = (from @group in features.GroupBy(f => f.Group).OrderBy(g => g.Key)
            let featuresInGroup = (from feature in @group
                let featureClaims = claimsByFeature[feature.Id].ToList()
                let canRead = featureClaims.Any(c => c.PermissionAction == PermissionAction.Read)
                let canWrite = featureClaims.Any(c => c.PermissionAction == PermissionAction.Write)
                let canDelete = featureClaims.Any(c => c.PermissionAction == PermissionAction.Delete)
                let customPermissions = featureClaims.Where(c => c.PermissionAction is null or PermissionAction.None)
                    .Select(c => c.ClaimValue!.Split('.').Last())
                    .ToList()
                let availableCustomPermissions = string.IsNullOrEmpty(feature.CustomPermissions)
                    ? []
                    : feature.CustomPermissions.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
                select new PermissionItemDto
                {
                    FeatureId = feature.Id,
                    Code = feature.Code,
                    Description = feature.Description,
                    Read = new PredefinedPermission<bool>(canRead),
                    Write = new PredefinedPermission<bool>(canWrite) ,
                    Delete = new PredefinedPermission<bool>(canDelete),
                    CustomPermissions = customPermissions,
                    AvailableCustomPermissions = availableCustomPermissions
                }).ToList()
            select new PermissionGroupDto { GroupName = @group.Key ?? "Unassigned", Order = @group.First().Order, Features = featuresInGroup }).ToList();

        return new PermissionSchemaDto
        {
            EntityId = entityId,
            EntityName = entityName,
            Groups = permissionGroups
        };
    }

    private record GenericClaimDto(Guid? FeatureId, PermissionAction? PermissionAction, string? ClaimValue);

    public async Task<bool> HasPermissionAsync(string userId, IEnumerable<string> userRoles, string featureCode, PermissionAction action)
    {
        if (!Guid.TryParse(userId, out var userGuid)) return false;

        var permissionClaimValue = $"Permissions.{featureCode}.{action}";

        var roleClaimsQuery = 
            from rc in dbContext.RoleClaims
            join r in dbContext.Roles on rc.RoleId equals r.Id
            where userRoles.Contains(r.Name) &&
                  rc.ClaimValue == permissionClaimValue &&
                  rc.ClaimType == "Permission"
            select 1;

        var userClaimsQuery = dbContext.UserClaims
            .Where(uc => uc.UserId == userGuid &&
                         uc.ClaimValue == permissionClaimValue &&
                         uc.ClaimType == "Permission")
            .Select(_ => 1);

        return await roleClaimsQuery.Union(userClaimsQuery).AnyAsync();
    }

    private static string GetComposedName(ApplicationUser user)
    {
        var name = $"{user.FirstName} {user.LastName}".Trim();

        return (string.IsNullOrWhiteSpace(name) ? user.UserName : $"{name} ( {user.UserName} )") ?? "Unassigned";
    }
}
