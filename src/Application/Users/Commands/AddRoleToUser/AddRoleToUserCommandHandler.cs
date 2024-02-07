using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.AddRoleToUser;

public class AddRoleToUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<AddRoleToUserCommandHandler> logger) : IRequestHandler<AddRoleToUserCommand, IdentityResult>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AddRoleToUserCommandHandler> _logger = logger;

    public async Task<IdentityResult> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception(message: "User not found to add in a role");
        }

        IdentityResult result = await _userManager.AddToRoleAsync(user, request.RoleName);

        if (result.Succeeded)
        {
            _logger.LogInformation($"Added user to role {request.RoleName} for user {user.UserName}");
        }
        return result;
    }
}
