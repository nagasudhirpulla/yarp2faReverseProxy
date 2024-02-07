using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.AddRoleToUser;

public class AddRoleToUserCommand : IRequest<IdentityResult>
{
    public string UserId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
