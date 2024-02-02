using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.UserRoles.Commands.CreateUserRole;

public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, IdentityResult>
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<CreateUserRoleCommandHandler> _logger;

    public CreateUserRoleCommandHandler(RoleManager<IdentityRole> roleManager, ILogger<CreateUserRoleCommandHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<IdentityResult> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
    {
        IdentityResult result = await _roleManager.CreateAsync(new IdentityRole { Name = request.Name });
        if (result.Succeeded)
        {
            _logger.LogInformation($"Created new role named {request.Name}");
        }

        return result;
    }
}
