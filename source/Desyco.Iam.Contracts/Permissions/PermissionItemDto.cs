namespace Desyco.Iam.Contracts.Permissions;

public record PermissionItemDto
{
    public Guid FeatureId { get; init; }

    public string Code { get; init; } = null!;

    public string Description { get; init; } = null!;

    public bool Read { get; init; }

    public bool Write { get; init; }

    public bool Delete { get; init; }

    public List<string> CustomPermissions { get; init; } = [];
}
