namespace Desyco.Iam.Contracts.Permissions;

public record FeatureDto
{
    public Guid Id { get; init; }

    public string Code { get; init; } = null!;

    public string Description { get; init; } = null!;

    public string? Group { get; init; }

    public int Order { get; init; }
}
