using Desyco.Iam.Contracts.Authentication;

namespace Desyco.Iam.Contracts.Permissions;

public record FeatureDto(
    Guid Id,
    string Code,
    string Description,
    string? Group,
    int Order);

public record RoleClaimDto(
    int Id,
    Guid RoleId,
    string ClaimType,
    string ClaimValue,
    Guid? FeatureId,
    PermissionAction? PermissionAction,
    string? Description);
