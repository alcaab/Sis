using Microsoft.AspNetCore.Identity;

namespace Desyco.Iam.Infrastructure.Persistence.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
    
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }
}
