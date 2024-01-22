using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<IdentityResult>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool IsTwoFactorEnabled { get; set; } = true;
    public string UserRole { get; set; } = SecurityConstants.GuestRoleString;
}
