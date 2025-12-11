namespace Desyco.Iam.Contracts.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, string firstName, string lastName, IEnumerable<string> roles);
}
