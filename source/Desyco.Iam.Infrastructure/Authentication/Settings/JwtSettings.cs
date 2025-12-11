namespace Desyco.Iam.Infrastructure.Authentication.Settings;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
    public int RefreshTokenExpiryDays { get; init; } // Nueva propiedad
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
}
