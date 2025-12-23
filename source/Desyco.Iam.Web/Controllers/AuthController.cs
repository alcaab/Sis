using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Infrastructure.Authentication.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Desyco.Iam.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IIdentityService identityService, IOptions<JwtSettings> jwtOptions) : ControllerBase
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await identityService.RegisterAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        SetRefreshTokenCookie(result.Token!.RefreshToken);

        // Remove RefreshToken from response body for security
        return Ok(result.Token with { RefreshToken = string.Empty });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await identityService.LoginAsync(request);

        if (!result.IsSuccess)
        {
            return Unauthorized(new { result.Errors });
        }

        SetRefreshTokenCookie(result.Token!.RefreshToken);

        return Ok(result.Token with { RefreshToken = string.Empty });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "No Refresh Token found in cookies." });
        }

        var result = await identityService.RefreshTokenAsync(string.Empty, refreshToken);

        if (!result.IsSuccess)
        {
            return Unauthorized(new { result.Errors });
        }

        SetRefreshTokenCookie(result.Token!.RefreshToken);

        return Ok(result.Token with { RefreshToken = string.Empty });
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies[AuthConstants.RefreshTokenCookieName];
        
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await identityService.RevokeTokenAsync(refreshToken);
        }
        
        Response.Cookies.Delete(AuthConstants.RefreshTokenCookieName);
        return Ok(new { message = "Logged out successfully" });
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            SameSite = SameSiteMode.Strict, // Important for CSRF protection
            Secure = true // Always send over HTTPS (ensure dev env uses https or handle locally)
        };

        Response.Cookies.Append(AuthConstants.RefreshTokenCookieName, refreshToken, cookieOptions);
    }
}