using Desyco.Iam.Contracts.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Desyco.Iam.Infrastructure.Authentication.Authorization;

public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Check if the policy name matches our permission pattern
        if (policyName.StartsWith("Permissions", StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            
            // Expected format: Permissions.{Feature}.{Action}
            var parts = policyName.Split('.');
            if (parts.Length == 3)
            {
                var featureCode = parts[1];
                var actionString = parts[2];
                
                if (Enum.TryParse<PermissionAction>(actionString, true, out var action))
                {
                    policy.AddRequirements(new PermissionRequirement(featureCode, action));
                    return policy.Build();
                }
            }
        }

        // Fallback to default behavior
        return await base.GetPolicyAsync(policyName);
    }
}
