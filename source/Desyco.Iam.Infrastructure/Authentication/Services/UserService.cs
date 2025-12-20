using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Contracts.Users;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Desyco.Iam.Infrastructure.Authentication.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<List<UserDto>> GetAllAsync()
    {
        return await userManager.Users
            .OrderBy(u => u.UserName)
            .Select(u => new UserDto(
                u.Id,
                u.UserName,
                u.Email,
                u.PhoneNumber,
                u.FirstName,
                u.LastName,
                u.LockoutEnabled
            ))
            .ToListAsync();
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return  null;
        }
        
        return new UserDto(
            user.Id,
            user.UserName,
            user.Email,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            user.LockoutEnabled
        )
        {
            Roles = (await userManager.GetRolesAsync(user)).OrderBy(e => e).ToList()
        };
    }

    public async Task<UserDto> CreateAsync(UserDto request)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            LockoutEnabled = request.LockoutEnabled
        };

        var result = await userManager.CreateAsync(user, string.Empty);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return new UserDto(
            user.Id,
            user.UserName,
            user.Email,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            user.LockoutEnabled
        );
    }

    public async Task UpdateAsync(Guid id, UserDto request)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        user.UserName = request.UserName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.LockoutEnabled = request.LockoutEnabled;

        if (request.Roles.Count > 0)
        {
            var roles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, roles.Except(request.Roles));
            await userManager.AddToRolesAsync(user, request.Roles.Except(roles));
        }

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new KeyNotFoundException($"User with ID {id} not found.");

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
