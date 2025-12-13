namespace Desyco.Iam.Contracts.Roles;

public record RoleDto(Guid Id, string Name, string? Description);

public record CreateRoleDto(string Name, string? Description);

public record UpdateRoleDto(string Name, string? Description);
