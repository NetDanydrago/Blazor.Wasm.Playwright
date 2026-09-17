namespace UserManagementDemo.Models;

public sealed record User(
    Guid Id,
    string FullName,
    string Email,
    string Role,
    bool IsActive,
    DateTimeOffset CreatedAt);