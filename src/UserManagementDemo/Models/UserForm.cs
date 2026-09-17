using System.ComponentModel.DataAnnotations;

namespace UserManagementDemo.Models;

public sealed class UserForm
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Full name must contain between 2 and 80 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Select a role.")]
    public string Role { get; set; } = "User";

    public bool IsActive { get; set; } = true;
}