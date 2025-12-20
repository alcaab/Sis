using System.Security.Claims;
using Desyco.Iam.Contracts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Desyco.Iam.Infrastructure.Authentication.Authorization;

public class PermissionAuthorizationHandler(IPermissionService permissionService)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Try standard ClaimTypes.NameIdentifier or "sub" or "uid"
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                     context.User.FindFirstValue("sub");

        if (userId == null)
        {
            return;
        }

        var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        if (await permissionService.HasPermissionAsync(userId, userRoles, requirement.FeatureCode, requirement.Action))
        {
            context.Succeed(requirement);
        }
    }
}
