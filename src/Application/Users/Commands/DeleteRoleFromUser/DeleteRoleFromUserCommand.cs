using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.DeleteRoleFromUser;

public class DeleteRoleFromUserCommand : IRequest<IdentityResult>
{
    public string UserId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
