namespace Desyco.Iam.Contracts.Permissions;

public record PermissionGroupDto
{
    public string GroupName { get; init; } = null!;

    public int Order { get; init; }

    public List<PermissionItemDto> Features { get; init; } = [];
}
