namespace Desyco.Iam.Contracts.Permissions;

public record PermissionItemDto
{
    public Guid FeatureId { get; init; }

    public required string Code { get; init; }

    public required string Description { get; init; }

    public required PredefinedPermission Read { get; init; }

    public required PredefinedPermission Write { get; init; }

    public required PredefinedPermission Delete { get; init; }

    public List<string> CustomPermissions { get; init; } = [];

    //public List<string> AvailableCustomPermissions { get; init; } = [];
    
    public Dictionary<string, PredefinedPermission> AvailableCustomPermissions { get; init; } = new();
}
