namespace Desyco.Iam.Contracts.Permissions;

public record PermissionSchemaDto
{
    public Guid EntityId { get; init; }

    public string EntityName { get; init; } = null!;

    public List<PermissionGroupDto> Groups { get; init; } = [];
}
