using System.Security.Claims;

namespace Desyco.ContractManager.Application.Common.Auth;

public interface IUserContext
{
    ClaimsPrincipal? Principal { get; }

    string ClaimType { get; }

    bool IsAuthenticated => Principal is { Identity.IsAuthenticated: true };

    IEnumerable<Claim> Claims => Principal?.Claims ?? [];

    // string? Subject => this.IsClientUser()
    //     ? Principal?.FindFirst("client_id")?.Value
    //     : Principal?.FindFirst("sub")?.Value;
    
    string? Subject { get; }

    bool IsClientUser();

    bool HasClaim(string claimType, string? claimValue = null)
    {
        if (Principal == null)
        {
            return false;
        }

        if (claimValue == null)
        {
            return Principal.FindFirst(claimType) != null;
        }

        var claims = Principal.FindAll(claimType);
        return claims.Select(claim => claim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries))
            .Any(values => values.Contains(claimValue, StringComparer.OrdinalIgnoreCase));
    }

    Claim? FindFirst(string claimType) =>
        Principal?.FindFirst(claimType);

    IEnumerable<Claim> FindAll(string claimType) =>
        Principal?.FindAll(claimType) ?? [];
}
