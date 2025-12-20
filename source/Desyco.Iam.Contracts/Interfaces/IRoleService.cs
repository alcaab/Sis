using Desyco.Iam.Contracts.Roles;

namespace Desyco.Iam.Contracts.Interfaces;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync();

    Task<List<string>> GetNamesAsync();
    
    Task<RoleDto?> GetByIdAsync(Guid id);
    
    Task<RoleDto> CreateAsync(RoleDto request);
    
    Task UpdateAsync(Guid id, RoleDto request);
    
    Task DeleteAsync(Guid id);
}
