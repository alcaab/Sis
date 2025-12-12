namespace Desyco.Iam.Contracts.Permissions;

public record RolePermissionSchemaDto
{
    public Guid RoleId { get; init; }

    public string RoleName { get; init; } = null!;

    public List<PermissionGroupDto> Groups { get; init; } = [];
}
