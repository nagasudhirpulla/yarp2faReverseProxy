using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UserRoles.Commands.CreateUserRole;

public class CreateUserRoleCommand : IRequest<IdentityResult>
{
    public string Name { get; set; } = string.Empty;
}
