using Desyco.Iam.Contracts.Roles;

namespace Desyco.Iam.Contracts.Interfaces;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();
    
    Task<RoleDto?> GetByIdAsync(Guid id);
    
    Task<RoleDto> CreateAsync(CreateRoleDto request);
    
    Task UpdateAsync(Guid id, UpdateRoleDto request);
    
    Task DeleteAsync(Guid id);
}
