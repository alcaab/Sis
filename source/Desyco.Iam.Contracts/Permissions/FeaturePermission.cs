using Desyco.Iam.Contracts.Authentication;

namespace Desyco.Iam.Contracts.Permissions;

public class FeaturePermission
{
    public Guid FeatureId { get; set; }

    public string FeatureCode { get; set; } = null!;

    public PermissionAction Action { get; set; }

    public bool IsGranted { get; set; }
}
