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
        var adminRole = await roleManager.FindByNameAsync("Admin");

        if (adminRole == null)
        {
            return;
        }

        var allFeatures = await dbContext.Features.ToListAsync();

        if (allFeatures.Count == 0)
        {
            return;
        }

        var defaultActions = new List<(PermissionAction action, string description)>
        {
            (PermissionAction.Read, string.Empty),
            (PermissionAction.Write, string.Empty),
            (PermissionAction.Delete, string.Empty)
        };

        foreach (var feature in allFeatures)
        {
            var actions = defaultActions.Concat(
                feature.CustomPermissions?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => (PermissionAction.None, e.Trim()))
                    .ToList()
                ?? []
            );

            foreach (var action in actions)
            {
                object actionName = action.Item1 == PermissionAction.None ? action.Item2 : action.Item1;
                var claimValue = $"Permissions.{feature.Code}.{actionName}";
                
                var exists = await dbContext.RoleClaims
                    .AnyAsync(rc => rc.RoleId == adminRole.Id &&
                                    rc.FeatureId == feature.Id &&
                                    rc.PermissionAction == action.Item1);

                if (!exists)
                {
                    await dbContext.RoleClaims.AddAsync(new ApplicationRoleClaim
                    {
                        RoleId = adminRole.Id,
                        ClaimType = "Permission",
                        ClaimValue = claimValue,
                        FeatureId = feature.Id,
                        PermissionAction = action.Item1,
                        Description = $"Auto-generated permission for Admin: {feature.Code} {action}"
                    });
                }
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
