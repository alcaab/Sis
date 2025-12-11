using Desyco.Iam.Contracts.Authentication;
using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace Desyco.Iam.Infrastructure.Authentication.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtTokenGenerator jwtTokenGenerator)
    : IIdentityService
{
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

        // Generate Token
        var token = jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            []); // No roles assigned initially

        return AuthenticationResult.Success(new TokenResponse(
            token,
            string.Empty, // Refresh token not implemented yet
            DateTime.UtcNow, // Simplified expiry
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            []));
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

        var token = jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email!,
            user.FirstName!,
            user.LastName!,
            roles);

        return AuthenticationResult.Success(new TokenResponse(
            token,
            string.Empty,
            DateTime.UtcNow,
            user.Id,
            user.Email!,
            user.FirstName!,
            user.LastName!,
            roles.ToList()));
    }
}
