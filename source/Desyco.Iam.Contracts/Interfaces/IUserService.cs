using Desyco.Iam.Contracts.Users;

namespace Desyco.Iam.Contracts.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    
    Task<UserDto?> GetByIdAsync(Guid id);
    
    Task<UserDto> CreateAsync(UserDto request);
    
    Task UpdateAsync(Guid id, UserDto request);
    
    Task DeleteAsync(Guid id);
}
