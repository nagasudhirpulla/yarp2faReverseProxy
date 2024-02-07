using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.DeleteRoleFromUser;

public class DeleteRoleFromUserCommandHandler(UserManager<ApplicationUser> userManager, ILogger<DeleteRoleFromUserCommandHandler> logger) : IRequestHandler<DeleteRoleFromUserCommand, IdentityResult>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<DeleteRoleFromUserCommandHandler> _logger = logger;

    public async Task<IdentityResult> Handle(DeleteRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new Exception(message: "User not found to remove role");
        }

        IdentityResult result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);

        if (result.Succeeded)
        {
            _logger.LogInformation($"Removed role {request.RoleName} from user {user.UserName}");
        }
        return result;
    }
}