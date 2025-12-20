using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Contracts.Roles;
using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// ReSharper disable NullableWarningSuppressionIsUsed

namespace Desyco.Iam.Infrastructure.Authentication.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
    public async Task<List<RoleDto>> GetAllAsync()
    {
        return await roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => new RoleDto(r.Id, r.Name!, r.Description))
            .ToListAsync();
    }
    
    public async Task<List<string>> GetNamesAsync()
    {
        return await roleManager.Roles
            .OrderBy(r => r.Name)
            .Select(r => r.Name!)
            .ToListAsync();
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        return role == null ? null : new RoleDto(role.Id, role.Name!, role.Description);
    }

    public async Task<RoleDto> CreateAsync(RoleDto request)
    {
        var role = new ApplicationRole(request.Name)
        {
            Description = request.Description
        };

        var result = await roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return new RoleDto(role.Id, role.Name!, role.Description);
    }

    public async Task UpdateAsync(Guid id, RoleDto request)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role == null) throw new KeyNotFoundException($"Role with ID {id} not found.");

        role.Name = request.Name;
        role.Description = request.Description;

        var result = await roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        if (role == null) throw new KeyNotFoundException($"Role with ID {id} not found.");

        var result = await roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
