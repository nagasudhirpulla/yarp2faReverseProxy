using FluentValidation;

namespace Application.Users.Commands.AddRoleToUser;

public class AddRoleToUserCommandValidator : AbstractValidator<AddRoleToUserCommand>
{
    public AddRoleToUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty();
    }
}
