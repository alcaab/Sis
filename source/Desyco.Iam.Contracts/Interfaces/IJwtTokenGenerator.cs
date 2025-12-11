namespace Desyco.Iam.Contracts.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, string Jti) GenerateToken(Guid userId, string email, string firstName, string lastName, IEnumerable<string> roles);
    string GenerateRefreshToken();
}
