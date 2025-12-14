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
    public async Task<List<FeatureDto>> GetAllFeaturesAsync()
    {
        var languageId = await GetLanguageIdAsync("");

        return await (from f in dbContext.Features
                      from t in dbContext.Translations
                                         .Where(t => t.Key == f.Description && t.LanguageId == languageId)
                                         .DefaultIfEmpty()
                      orderby f.Group, f.Order
                      select new FeatureDto
                      {
                          Id = f.Id,
                          Code = f.Code,
                          Description = t.Value ?? f.Description,
                          Group = f.Group,
                          Order = f.Order
                      })
                      .ToListAsync();
    }

    public async Task<List<RoleClaimDto>> GetRoleClaimsAsync(Guid roleId)
    {
        return await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId) // Get all role claims, not just granular
            .Select(rc => new RoleClaimDto
            {
                Id = rc.Id,
                RoleId = rc.RoleId,
                ClaimType = rc.ClaimType!,
                ClaimValue = rc.ClaimValue!,
                FeatureId = rc.FeatureId,
                PermissionAction = rc.PermissionAction,
                Description = rc.Description
            })
            .ToListAsync();
    }

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
                    (up.Action == PermissionAction.None && (claim.PermissionAction == null || claim.PermissionAction == PermissionAction.None) &&
                     !string.IsNullOrEmpty(up.CustomActionName) &&
                     claim.ClaimValue!.EndsWith($".{up.CustomActionName}", StringComparison.OrdinalIgnoreCase))
                )
            );

            // If no match found in request, or found but explicitly revoked (IsGranted=false) -> Remove
            if (match == null || !match.IsGranted)
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
            bool exists = existingClaims.Any(claim => 
                claim.FeatureId == up.FeatureId &&
                (
                    (up.Action != PermissionAction.None && up.Action == claim.PermissionAction) ||
                    (up.Action == PermissionAction.None && (claim.PermissionAction == null || claim.PermissionAction == PermissionAction.None) &&
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

        var languageId = await GetLanguageIdAsync("");

        // Fetch ALL features with translated descriptions
        var allFeaturesDto = await (from f in dbContext.Features
                                    from t in dbContext.Translations
                                                       .Where(t => t.Key == f.Description && t.LanguageId == languageId)
                                                       .DefaultIfEmpty()
                                    orderby f.Group, f.Order
                                    select new FeatureDto
                                    {
                                        Id = f.Id,
                                        Code = f.Code,
                                        Description =  t.Value ?? f.Description,
                                        Group = f.Group,
                                        Order = f.Order,
                                        CustomPermissions = f.CustomPermissions
                                    }).ToListAsync();

        var claims = await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .Select(rc => new GenericClaimDto(rc.FeatureId, rc.PermissionAction, rc.ClaimValue))
            .ToListAsync();

        // BuildPermissionSchema now receives FeatureDto
        return BuildPermissionSchema(roleId, role.Name!, allFeaturesDto, claims);
    }
    
    public async Task<PermissionSchemaDto> GetPermissionSchemaForUserAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found.");
        }
        
        var languageId = await GetLanguageIdAsync("");

        // Fetch ALL features with translated descriptions
        var allFeaturesDto = await (from f in dbContext.Features
                                    from t in dbContext.Translations
                                                       .Where(t => t.Key == f.Description && t.LanguageId == languageId)
                                                       .DefaultIfEmpty()
                                    orderby f.Group, f.Order
                                    select new FeatureDto
                                    {
                                        Id = f.Id,
                                        Code = f.Code,
                                        Description =  t.Value ?? f.Description,
                                        Group = f.Group,
                                        Order = f.Order,
                                        CustomPermissions = f.CustomPermissions
                                    }).ToListAsync();

        var claims = await dbContext.UserClaims
            .Where(uc => uc.UserId == userId)
            .Select(uc => new GenericClaimDto(uc.FeatureId, uc.PermissionAction, uc.ClaimValue))
            .ToListAsync();

        // BuildPermissionSchema now receives FeatureDto
        return BuildPermissionSchema(userId, user.Email!, allFeaturesDto, claims);
    }

    private static PermissionSchemaDto BuildPermissionSchema(
        Guid entityId, 
        string entityName, 
        List<FeatureDto> features, // Changed from List<Feature> to List<FeatureDto>
        List<GenericClaimDto> claims)
    {
        var permissionGroups = new List<PermissionGroupDto>();

        // Optimization: Create a lookup for O(1) access to claims by FeatureId
        var claimsByFeature = claims
            .Where(c => c.FeatureId.HasValue)
            .ToLookup(c => c.FeatureId!.Value);

        foreach (var group in features.GroupBy(f => f.Group).OrderBy(g => g.Key))
        {
            var featuresInGroup = (from feature in @group
                let featureClaims = claimsByFeature[feature.Id].ToList()
                let canRead = featureClaims.Any(c => c.PermissionAction == PermissionAction.Read)
                let canWrite = featureClaims.Any(c => c.PermissionAction == PermissionAction.Write)
                let canDelete = featureClaims.Any(c => c.PermissionAction == PermissionAction.Delete)
                let customPermissions = featureClaims.Where(c => c.PermissionAction == null || c.PermissionAction == PermissionAction.None)
                    .Select(c => c.ClaimValue!.Split('.').Last()) // Extract "Print" from "Permissions.Feature.Print"
                    .ToList()
                let availableCustomPermissions = string.IsNullOrEmpty(feature.CustomPermissions) 
                    ? [] 
                    : feature.CustomPermissions.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
                select new PermissionItemDto
                {
                    FeatureId = feature.Id,
                    Code = feature.Code,
                    Description = feature.Description, 
                    Read = canRead,
                    Write = canWrite,
                    Delete = canDelete,
                    CustomPermissions = customPermissions,
                    AvailableCustomPermissions = availableCustomPermissions
                }).ToList();

            permissionGroups.Add(new PermissionGroupDto
            {
                GroupName = group.Key ?? "Unassigned", // Handle null groups
                Order = group.First().Order, // Assume first feature's order represents group order
                Features = featuresInGroup
            });
        }

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
        // Convert user ID string to Guid
        if (!Guid.TryParse(userId, out var userGuid)) return false;
        
        // Construct the expected Claim Value string (e.g., "Permissions.AcademicYears.Read")
        // This avoids querying the Features table to get the ID.
        var permissionClaimValue = $"Permissions.{featureCode}.{action}";

        // Optimized query using LINQ Query Syntax to join Roles without navigation properties
        // and filtering by ClaimValue (string) instead of FeatureId (Guid)
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
    
    private async Task<int> GetLanguageIdAsync(string languageCode, CancellationToken cancellationToken = default)
    {
        // var languageId = await dbContext.Languages
        //     .Where(l => l.Code == languageCode)
        //     .Select(l => l.Id)
        //     .FirstOrDefaultAsync(cancellationToken);
        return 2;
    }
}
