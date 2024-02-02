using MediatR;

namespace Application.UserRoles.Commands.DeleteUserRole;

public class DeleteUserRoleCommand : IRequest<List<string>>
{
    public string Id { get; set; } = string.Empty;
}
