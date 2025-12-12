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
    RoleManager<ApplicationRole> roleManager) : IPermissionService
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
            .Where(rc => rc.RoleId == roleId && rc.FeatureId != null) // Only granular permissions
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

        // Get existing claims for the role
        var existingClaims = await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId && rc.FeatureId != null)
            .ToListAsync();

        // Remove old permissions that are no longer granted
        foreach (var existingClaim in existingClaims)
        {
            var matchingUpdate = updatedPermissions.FirstOrDefault(up =>
                up.FeatureId == existingClaim.FeatureId &&
                up.Action == existingClaim.PermissionAction);

            if (matchingUpdate == null || !matchingUpdate.IsGranted)
            {
                dbContext.RoleClaims.Remove(existingClaim);
            }
        }

        // Add new permissions
        foreach (var updatedPermission in updatedPermissions)
        {
            if (updatedPermission.IsGranted)
            {
                var exists = existingClaims.Any(rc =>
                    rc.FeatureId == updatedPermission.FeatureId &&
                    rc.PermissionAction == updatedPermission.Action);

                if (!exists)
                {
                    var newClaim = new ApplicationRoleClaim
                    {
                        Id = 0, // Id will be set by DB
                        RoleId = roleId,
                        ClaimType = "Permission", // Standard claim type for permissions
                        ClaimValue = $"Permissions.{updatedPermission.FeatureCode}.{updatedPermission.Action}", // Example: Permissions.AcademicYears.Read
                        FeatureId = updatedPermission.FeatureId,
                        PermissionAction = updatedPermission.Action,
                        Description = $"Claim for {updatedPermission.FeatureCode} {updatedPermission.Action}" // Placeholder
                    };
                    await dbContext.RoleClaims.AddAsync(newClaim);
                }
            }
        }

        await dbContext.SaveChangesAsync();
    }
    
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