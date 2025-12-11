using Desyco.Iam.Contracts.Authentication;

namespace Desyco.Iam.Contracts.Interfaces;

public interface IIdentityService
{
    Task<AuthenticationResult> RegisterAsync(RegisterRequest request);
    Task<AuthenticationResult> LoginAsync(LoginRequest request);
    Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
}
