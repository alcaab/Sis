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
        return await dbContext.Features
            .OrderBy(f => f.Group)
            .ThenBy(f => f.Order)
            .Select(f => new FeatureDto
            {
                Id = f.Id,
                Code = f.Code,
                Description = f.Description,
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

        // Get existing claims for the role (including non-granular for preservation)
        var existingClaims = await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .ToListAsync();

        // Separate granular claims
        var existingGranularClaims = existingClaims.Where(rc => rc.FeatureId != null).ToList();
        var otherClaims = existingClaims.Where(rc => rc.FeatureId == null).ToList(); // Claims not related to features

        // Remove old granular permissions that are no longer granted
        foreach (var existingClaim in from existingClaim in existingGranularClaims let matchingUpdate = updatedPermissions.FirstOrDefault(up =>
                     up.FeatureId == existingClaim.FeatureId &&
                     up.Action == existingClaim.PermissionAction) where matchingUpdate is not { IsGranted: true } select existingClaim)
        {
            dbContext.RoleClaims.Remove(existingClaim);
        }

        // Add new granular permissions
        foreach (var newClaim in from updatedPermission in updatedPermissions where updatedPermission.IsGranted let exists = existingGranularClaims.Any(rc =>
                     rc.FeatureId == updatedPermission.FeatureId &&
                     rc.PermissionAction == updatedPermission.Action) where !exists select new ApplicationRoleClaim
                 {
                     Id = 0, // Id will be set by DB
                     RoleId = roleId,
                     ClaimType = "Permission", // Standard claim type for permissions
                     ClaimValue = $"Permissions.{updatedPermission.FeatureCode}.{updatedPermission.Action}", // Example: Permissions.AcademicYears.Read
                     FeatureId = updatedPermission.FeatureId,
                     PermissionAction = updatedPermission.Action,
                     Description = $"Claim for {updatedPermission.FeatureCode} {updatedPermission.Action}" // Placeholder
                 })
        {
            await dbContext.RoleClaims.AddAsync(newClaim);
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

        // Changed logic: Fetch ALL features, not just assigned ones.
        // This allows the UI to show the full catalog and let users grant permissions freely.
        var allFeatures = await dbContext.Features
            .OrderBy(f => f.Group)
            .ThenBy(f => f.Order)
            .ToListAsync();

        var claims = await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .Select(rc => new GenericClaimDto(rc.FeatureId, rc.PermissionAction, rc.ClaimValue))
            .ToListAsync();

        return BuildPermissionSchema(roleId, role.Name!, allFeatures, claims);
    }
    
    public async Task<PermissionSchemaDto> GetPermissionSchemaForUserAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found.");
        }

        var allFeatures = await dbContext.Features
            .OrderBy(f => f.Group)
            .ThenBy(f => f.Order)
            .ToListAsync();

        var claims = await dbContext.UserClaims
            .Where(uc => uc.UserId == userId)
            .Select(uc => new GenericClaimDto(uc.FeatureId, uc.PermissionAction, uc.ClaimValue))
            .ToListAsync();

        return BuildPermissionSchema(userId, user.Email!, allFeatures, claims);
    }

    private static PermissionSchemaDto BuildPermissionSchema(
        Guid entityId, 
        string entityName, 
        List<Feature> features, 
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
                select new PermissionItemDto
                {
                    FeatureId = feature.Id,
                    Code = feature.Code,
                    Description = feature.Description,
                    Read = canRead,
                    Write = canWrite,
                    Delete = canDelete,
                    CustomPermissions = customPermissions
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
}
