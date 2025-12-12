using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Iam.Infrastructure.Seeders;

public class PermissionSeeder(
    IamDbContext dbContext,
    RoleManager<ApplicationRole> roleManager)
{
    public async Task SeedAsync()
    {
        // 1. Get Admin Role
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole == null) return; // Should not happen if SecuritySeeder ran first

        // 2. Get All Features
        var allFeatures = await dbContext.Features.ToListAsync();
        if (allFeatures.Count == 0) return;

        // 3. Define Actions to Grant (Full Access)
        var actions = new[] { PermissionAction.Read, PermissionAction.Write, PermissionAction.Delete };

        // 4. Assign Permissions
        foreach (var feature in allFeatures)
        {
            foreach (var action in actions)
            {
                var claimValue = $"Permissions.{feature.Code}.{action}";
                
                // Check if permission already exists to avoid duplicates
                var exists = await dbContext.RoleClaims
                    .AnyAsync(rc => rc.RoleId == adminRole.Id && 
                                    rc.FeatureId == feature.Id && 
                                    rc.PermissionAction == action);

                if (!exists)
                {
                    await dbContext.RoleClaims.AddAsync(new ApplicationRoleClaim
                    {
                        RoleId = adminRole.Id,
                        ClaimType = "Permission",
                        ClaimValue = claimValue,
                        FeatureId = feature.Id,
                        PermissionAction = action,
                        Description = $"Auto-generated permission for Admin: {feature.Code} {action}"
                    });
                }
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
