namespace Desyco.Iam.Contracts.Permissions;

// DTO Raíz
public record PermissionSchemaDto
{
    public Guid EntityId { get; init; }

    public string EntityName { get; init; } = null!;

    public List<PermissionGroupDto> Groups { get; init; } = [];
}
