using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Infrastructure.Authentication.Settings;
using Desyco.Iam.Infrastructure.Persistence.Context;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Desyco.Iam.Infrastructure.Authentication.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtTokenGenerator jwtTokenGenerator,
    IamDbContext dbContext,
    IOptions<JwtSettings> jwtOptions)
    : IIdentityService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<AuthenticationResult> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return AuthenticationResult.Fail("User with given email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return AuthenticationResult.Fail(result.Errors.Select(e => e.Description).ToArray());
        }

        // Generate Tokens
        return await GenerateAuthenticationResultAsync(user, []);
    }

    public async Task<AuthenticationResult> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return AuthenticationResult.Fail("Invalid email or password.");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
             return AuthenticationResult.Fail("Invalid email or password.");
        }
        
        var roles = await userManager.GetRolesAsync(user);

        return await GenerateAuthenticationResultAsync(user, roles);
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
    {
        // 1. Get stored Refresh Token
        var storedToken = await dbContext.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Token == refreshToken);

        // 2. Validate existence
        if (storedToken is null)
        {
            return AuthenticationResult.Fail("Invalid Refresh Token.");
        }

        // 3. Validate expiration
        if (storedToken.ExpiryDate < DateTime.UtcNow)
        {
            return AuthenticationResult.Fail("Refresh Token expired.");
        }

        // 4. Validate manual invalidation
        if (storedToken.Invalidated)
        {
             return AuthenticationResult.Fail("Invalid Refresh Token.");
        }

        // 5. THEFT DETECTION (Rotation)
        // If a token marked as USED is reused, it means it was stolen.
        // We must invalidate ALL tokens of this user for security.
        if (storedToken.Used)
        {
            storedToken.Invalidated = true;
            // Optionally: Invalidate all other tokens for this user
            var allUserTokens = await dbContext.RefreshTokens
                .Where(x => x.UserId == storedToken.UserId)
                .ToListAsync();
            
            foreach(var t in allUserTokens)
            {
                t.Invalidated = true;
            }
            
            await dbContext.SaveChangesAsync();
            
            return AuthenticationResult.Fail("Security Alert: Token reuse detected. Please login again.");
        }

        // 6. Validate JTI match (Optional but recommended)
        // We assume 'token' passed is the AccessToken, but we might not need to parse it if we trust the Refresh Token flow.
        // If we want to link them strictly:
        // var jti = GetJtiFromToken(token);
        // if (storedToken.JwtId != jti) return Fail("Token mismatch");
        // For now, we skip parsing the expired access token to keep it simple, 
        // relying on the fact that the RefreshToken is the primary credential here.

        // 7. Mark current token as USED (Rotation)
        storedToken.Used = true;
        dbContext.RefreshTokens.Update(storedToken);
        await dbContext.SaveChangesAsync();

        // 8. Generate NEW tokens
        var user = storedToken.User;
        var roles = await userManager.GetRolesAsync(user);

        return await GenerateAuthenticationResultAsync(user, roles);
    }

    public async Task RevokeTokenAsync(string refreshToken)
    {
        var storedToken = await dbContext.RefreshTokens
            .SingleOrDefaultAsync(x => x.Token == refreshToken);

        if (storedToken is null)
        {
            return;
        }

        storedToken.Invalidated = true;
        dbContext.RefreshTokens.Update(storedToken);
        await dbContext.SaveChangesAsync();
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(ApplicationUser user, IList<string> roles)
    {
        // Generate Access Token (and get Jti)
        var (accessToken, jti) = jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email!,
            user.FirstName!,
            user.LastName!,
            roles);

        // Generate Refresh Token
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            JwtId = jti,
            Token = refreshToken,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            Used = false,
            Invalidated = false,
            UserId = user.Id
        };

        await dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
        await dbContext.SaveChangesAsync();

        return AuthenticationResult.Success(new TokenResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            user.Id,
            user.Email!,
            user.FirstName!,
            user.LastName!,
            roles.ToList()));
    }
}