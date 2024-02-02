using FluentValidation;

namespace Application.UserRoles.Commands.CreateUserRole;

public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
{
    public CreateUserRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
