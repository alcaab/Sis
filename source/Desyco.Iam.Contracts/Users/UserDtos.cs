namespace Desyco.Iam.Contracts.Users;

public record UserDto(
    Guid Id,
    string? UserName,
    string? Email,
    string? PhoneNumber,
    string? FirstName,
    string? LastName,
    bool LockoutEnabled
)
{
    public ICollection<string> Roles { get; init; } = [];
}

