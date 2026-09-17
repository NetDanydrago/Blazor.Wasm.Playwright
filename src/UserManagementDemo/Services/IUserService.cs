using UserManagementDemo.Models;

namespace UserManagementDemo.Services;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User> CreateAsync(UserForm form);
    Task<User?> UpdateAsync(Guid id, UserForm form);
    Task<bool> DeleteAsync(Guid id);
}