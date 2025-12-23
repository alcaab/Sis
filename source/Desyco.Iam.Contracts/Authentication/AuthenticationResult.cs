namespace Desyco.Iam.Contracts.Authentication;

public record AuthenticationResult(
    TokenResponse? Token = null,
    bool IsSuccess = false,
    string[]? Errors = null)
{
    public static AuthenticationResult Success(TokenResponse token) => new(Token: token, IsSuccess: true);
    
    public static AuthenticationResult Fail(params string[] errors) => new(IsSuccess: false, Errors: errors);
}
