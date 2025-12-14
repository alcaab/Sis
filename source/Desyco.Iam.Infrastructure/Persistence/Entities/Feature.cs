namespace Desyco.Iam.Infrastructure.Persistence.Entities;

public class Feature
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? Group { get; set; } 

    public int Order { get; set; }
    
    public string? CustomPermissions { get; set; }
}
