namespace Desyco.Iam.Infrastructure.Persistence.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } = null!;

    public string JwtId { get; set; } = null!; // Jti del Access Token asociado

    public DateTime CreationDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool Used { get; set; }

    public bool Invalidated { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
