using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.UserRoles.Commands.DeleteUserRole;

public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, List<string>>
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<DeleteUserRoleCommandHandler> _logger;

    public DeleteUserRoleCommandHandler(RoleManager<IdentityRole> roleManager, ILogger<DeleteUserRoleCommandHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<List<string>> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = [];
        IdentityRole? role = await _roleManager.FindByIdAsync(request.Id);

        if (role == null)
        {
            errors.Add($"Role not found with id {request.Id}");
        }
        else if (SecurityConstants.GetRoles().Any(r => r.Equals(role.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            errors.Add($"The Role {role.Name} can not be deleted since it is a reserved role...");
        }
        else
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                foreach (IdentityError err in result.Errors)
                {
                    errors.Add(err.Description);
                }
            }
        }

        return errors;
    }
}