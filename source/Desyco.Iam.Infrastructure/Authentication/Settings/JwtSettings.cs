namespace Desyco.Iam.Infrastructure.Authentication.Settings;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
}
