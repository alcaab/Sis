using Desyco.Iam.Contracts.Authentication;

namespace Desyco.Iam.Contracts.Permissions;

public record RoleClaimDto
{
    public int Id { get; init; }

    public Guid RoleId { get; init; }

    public string ClaimType { get; init; } = null!;

    public string ClaimValue { get; init; } = null!;

    public Guid? FeatureId { get; init; }

    public PermissionAction? PermissionAction { get; init; }

    public string? Description { get; init; }
}
