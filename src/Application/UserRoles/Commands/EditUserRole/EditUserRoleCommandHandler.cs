using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.UserRoles.Commands.EditUserRole;

public class EditUserRoleCommandHandler : IRequestHandler<EditUserRoleCommand, List<string>>
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<EditUserRoleCommandHandler> _logger;

    public EditUserRoleCommandHandler(RoleManager<IdentityRole> roleManager, ILogger<EditUserRoleCommandHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<List<string>> Handle(EditUserRoleCommand request, CancellationToken cancellationToken)
    {
        List<string> errors = [];
        IdentityRole? existingRole = await _roleManager.FindByIdAsync(request.Id);
        if (existingRole == null)
        {
            errors.Add($"Unable to find role with id {request.Id}");
            return errors;
        }

        List<IdentityError> identityErrors = [];

        // change role name if changed
        if (request.Name != existingRole.Name)
        {
            existingRole.Name = request.Name;
            IdentityResult roleNameChangeResult = await _roleManager.UpdateAsync(existingRole);
            if (roleNameChangeResult.Succeeded)
            {
                _logger.LogInformation("Role name changed");
            }
            else
            {
                identityErrors.AddRange(roleNameChangeResult.Errors);
            }
        }

        foreach (IdentityError iError in identityErrors)
        {
            errors.Add(iError.Description);
        }
        return errors;
    }
}
