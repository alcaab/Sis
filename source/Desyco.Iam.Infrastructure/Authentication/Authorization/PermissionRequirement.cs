using Desyco.Iam.Contracts.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Desyco.Iam.Infrastructure.Authentication.Authorization;

public class PermissionRequirement(string featureCode, PermissionAction action) : IAuthorizationRequirement
{
    public string FeatureCode { get; } = featureCode;

    public PermissionAction Action { get; } = action;
}
