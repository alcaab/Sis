using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Contracts.Permissions;
using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// ReSharper disable NullableWarningSuppressionIsUsed

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

        var existingClaims = await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId && rc.FeatureId != null)
            .ToListAsync();

        var claimsToRemove = (from claim in existingClaims
            let match = updatedPermissions.FirstOrDefault(up =>
                up.FeatureId == claim.FeatureId &&
                ((up.Action != PermissionAction.None && up.Action == claim.PermissionAction) ||
                 (up.Action == PermissionAction.None && claim.PermissionAction is null or PermissionAction.None &&
                  !string.IsNullOrEmpty(up.CustomActionName) &&
                  claim.ClaimValue!.EndsWith($".{up.CustomActionName}", StringComparison.OrdinalIgnoreCase))))
            where match is not { IsGranted: true }
            select claim).ToList();

        if (claimsToRemove.Count != 0)
        {
            dbContext.RoleClaims.RemoveRange(claimsToRemove);
        }

        foreach (var up in updatedPermissions.Where(u => u.IsGranted))
        {
            var exists = existingClaims.Any(claim =>
                claim.FeatureId == up.FeatureId &&
                (
                    (up.Action != PermissionAction.None && up.Action == claim.PermissionAction) ||
                    (up.Action == PermissionAction.None && claim.PermissionAction is null or PermissionAction.None &&
                     !string.IsNullOrEmpty(up.CustomActionName) &&
                     claim.ClaimValue!.EndsWith($".{up.CustomActionName}", StringComparison.OrdinalIgnoreCase))
                )
            );

            if (exists)
            {
                continue;
            }

            var actionName = up.Action != PermissionAction.None ? up.Action.ToString() : up.CustomActionName;

            await dbContext.RoleClaims.AddAsync(new ApplicationRoleClaim
            {
                RoleId = roleId,
                ClaimType = "Permission",
                ClaimValue = $"Permissions.{up.FeatureCode}.{actionName}",
                FeatureId = up.FeatureId,
                PermissionAction = up.Action,
                Description = $"Claim for {up.FeatureCode} {actionName}"
            });
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
                Description = f.Description,
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
        
        var userClaimsQuery = dbContext.UserClaims
            .Where(uc => uc.UserId == userId)
            .Select(uc => new 
            { 
                uc.FeatureId, 
                uc.PermissionAction, 
                uc.ClaimValue, 
                IsInherited = false
            });
        
        var roleClaimsQuery = from ur in dbContext.UserRoles
            join rc in dbContext.RoleClaims on ur.RoleId equals rc.RoleId
            where ur.UserId == userId
            group rc by new { rc.FeatureId, rc.PermissionAction, rc.ClaimValue } into g
            select new 
            {
                g.Key.FeatureId,
                g.Key.PermissionAction,
                g.Key.ClaimValue,
                IsInherited = true
            };
        
        var claims = await userClaimsQuery
            .Concat(roleClaimsQuery) 
            .GroupBy(x => new { x.FeatureId, x.PermissionAction, x.ClaimValue })
            .Select(g => new GenericClaimDto(g.Key.FeatureId, g.Key.PermissionAction, g.Key.ClaimValue)
            {
                Inherited = g.Any(x => x.IsInherited)
            })
            .ToListAsync();

        return BuildPermissionSchema(userId, GetComposedName(user), allFeaturesDto, claims);
    }

    public async Task<bool> HasPermissionAsync(string userId, IEnumerable<string> userRoles, string featureCode,
        PermissionAction action)
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

    private static PermissionSchemaDto BuildPermissionSchema(
        Guid entityId,
        string entityName,
        List<FeatureDto> features,
        List<GenericClaimDto> claims)
    {
        var claimsByFeature = claims
            .Where(c => c.FeatureId.HasValue)
            .ToLookup(c => c.FeatureId!.Value);

        var permissionGroups = features
            .GroupBy(f => f.Group)
            .OrderBy(g => g.Key)
            .Select(group => new PermissionGroupDto
            {
                GroupName = group.Key ?? "Unassigned",
                Order = group.First().Order,
                Features = group.Select(feature =>
                {
                    var featureClaims = claimsByFeature[feature.Id];

                    PredefinedPermission canRead = new(false), canWrite = new(false), canDelete = new(false);
                    var customPermissions = new List<string>();
                    var availablePermissionsDic = ParseCustomPermissions(feature.CustomPermissions);

                    foreach (var claim in featureClaims)
                    {
                        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                        switch (claim.PermissionAction)
                        {
                            case PermissionAction.Read:
                                canRead = new PredefinedPermission(true) { Inherited = claim.Inherited };
                                break;
                            case PermissionAction.Write:
                                canWrite = new PredefinedPermission(true) { Inherited = claim.Inherited };
                                break;
                            case PermissionAction.Delete:
                                canDelete = new PredefinedPermission(true) { Inherited = claim.Inherited };
                                break;
                            case PermissionAction.None:
                            case null:
                                if (!string.IsNullOrEmpty(claim.ClaimValue))
                                {
                                    var claimValue = claim.ClaimValue.Split('.').Last();

                                    if (availablePermissionsDic.TryGetValue(claimValue, out var permission))
                                    {
                                        customPermissions.Add(claimValue);
                                        permission.Inherited = claim.Inherited;
                                    }
                                }

                                break;
                        }
                    }

                    return new PermissionItemDto
                    {
                        FeatureId = feature.Id,
                        Code = feature.Code,
                        Description = feature.Description,
                        Read = canRead,
                        Write = canWrite,
                        Delete = canDelete,
                        CustomPermissions = customPermissions,
                        AvailableCustomPermissions = availablePermissionsDic
                    };
                }).ToList()
            }).ToList();

        return new PermissionSchemaDto { EntityId = entityId, EntityName = entityName, Groups = permissionGroups };
    }

    private static Dictionary<string, PredefinedPermission> ParseCustomPermissions(string? customPermissions)
    {
        if (string.IsNullOrWhiteSpace(customPermissions)) return new Dictionary<string, PredefinedPermission>();

        return customPermissions
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToDictionary(k => k, name => new PredefinedPermission(false) { Name = name });
    }

    private static string GetComposedName(ApplicationUser user)
    {
        var name = $"{user.FirstName} {user.LastName}".Trim();

        return (string.IsNullOrWhiteSpace(name) ? user.UserName : $"{name} ( {user.UserName} )") ?? "Unassigned";
    }

    private record GenericClaimDto(Guid? FeatureId, PermissionAction? PermissionAction, string? ClaimValue)
    {
        public bool Inherited { get; init; }
    }
}
