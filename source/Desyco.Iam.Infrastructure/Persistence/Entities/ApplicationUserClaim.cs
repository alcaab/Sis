using Desyco.Iam.Contracts.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Desyco.Iam.Infrastructure.Persistence.Entities;

public class ApplicationUserClaim : IdentityUserClaim<Guid>
{
    public Guid? FeatureId { get; set; }

    public PermissionAction? PermissionAction { get; set; }

    public string? Description { get; set; }
}
