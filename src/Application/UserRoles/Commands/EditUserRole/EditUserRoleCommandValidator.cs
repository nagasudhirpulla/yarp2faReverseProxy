using FluentValidation;

namespace Application.UserRoles.Commands.EditUserRole;

public class EditUserRoleCommandValidator : AbstractValidator<EditUserRoleCommand>
{
    public EditUserRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
