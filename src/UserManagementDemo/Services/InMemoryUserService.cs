using UserManagementDemo.Models;

namespace UserManagementDemo.Services;

public sealed class InMemoryUserService : IUserService
{
    private readonly List<User> users =
    [
        new(Guid.Parse("8d20aa0d-20f0-474b-aa84-5add0028ebd8"), "Ana Martinez", "ana@example.com", "Administrator", true, DateTimeOffset.Now.AddDays(-18)),
        new(Guid.Parse("125643d5-4676-46c4-82d1-1b5f9528a791"), "Carlos Ruiz", "carlos@example.com", "Editor", true, DateTimeOffset.Now.AddDays(-9)),
        new(Guid.Parse("816855bb-8748-451f-b853-68926c092113"), "Lucia Torres", "lucia@example.com", "User", false, DateTimeOffset.Now.AddDays(-3))
    ];

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        await SimulateLatencyAsync();
        return users.OrderBy(user => user.FullName).ToArray();
    }

    public async Task<User> CreateAsync(UserForm form)
    {
        await SimulateLatencyAsync();
        var user = new User(Guid.NewGuid(), form.FullName.Trim(), form.Email.Trim().ToLowerInvariant(), form.Role, form.IsActive, DateTimeOffset.Now);
        users.Add(user);
        return user;
    }

    public async Task<User?> UpdateAsync(Guid id, UserForm form)
    {
        await SimulateLatencyAsync();
        var index = users.FindIndex(user => user.Id == id);
        if (index < 0)
        {
            return null;
        }

        var updatedUser = users[index] with
        {
            FullName = form.FullName.Trim(),
            Email = form.Email.Trim().ToLowerInvariant(),
            Role = form.Role,
            IsActive = form.IsActive
        };

        users[index] = updatedUser;
        return updatedUser;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await SimulateLatencyAsync();
        return users.RemoveAll(user => user.Id == id) > 0;
    }

    private static Task SimulateLatencyAsync() => Task.Delay(180);
}