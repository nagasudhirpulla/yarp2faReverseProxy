using FluentValidation;

namespace Application.Users.Commands.DeleteRoleFromUser;

public class DeleteRoleFromUserCommandValidator : AbstractValidator<DeleteRoleFromUserCommand>
{
    public DeleteRoleFromUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty();
    }
}