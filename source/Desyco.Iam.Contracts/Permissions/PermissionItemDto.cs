namespace Desyco.Iam.Contracts.Permissions;

public record PermissionItemDto
{
    public Guid FeatureId { get; init; }

    public required string Code { get; init; }

    public required string Description { get; init; }

    public required PredefinedPermission<bool> Read { get; init; }

    public required PredefinedPermission<bool> Write { get; init; }

    public required PredefinedPermission<bool> Delete { get; init; }

    public List<string> CustomPermissions { get; init; } = [];

    public List<string> AvailableCustomPermissions { get; init; } = [];
}
