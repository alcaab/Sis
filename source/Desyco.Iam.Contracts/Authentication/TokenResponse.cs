namespace Desyco.Iam.Contracts.Authentication;

public record TokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiryDate,
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    List<string> Roles);
