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
            .Select(f => new FeatureDto(f.Id, f.Code, f.Description, f.Group, f.Order))
            .ToListAsync();
    }

    public async Task<List<RoleClaimDto>> GetRoleClaimsAsync(Guid roleId)
    {
        return await dbContext.RoleClaims
            .Where(rc => rc.RoleId == roleId && rc.FeatureId != null) // Only granular permissions
            .Select(rc => new RoleClaimDto(rc.Id, rc.RoleId, rc.ClaimType!, rc.ClaimValue!, rc.FeatureId, rc.PermissionAction, rc.Description))
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
    
    public async Task<bool> HasPermissionAsync(string userId, string featureCode, PermissionAction action)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var userRoles = await userManager.GetRolesAsync(user);
        var roleIds = await dbContext.Roles
            .Where(r => userRoles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();
        
        var feature = await dbContext.Features.SingleOrDefaultAsync(f => f.Code == featureCode);
        if (feature == null) return false;

        // Check if any of the user's roles have the specific permission
        var hasRolePermission = await dbContext.RoleClaims
            .AnyAsync(rc => roleIds.Contains(rc.RoleId) &&
                            rc.FeatureId == feature.Id &&
                            rc.PermissionAction == action);

        if (hasRolePermission) return true;

        return false;
    }
}