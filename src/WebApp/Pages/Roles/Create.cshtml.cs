using Application.UserRoles.Commands.CreateUserRole;
using Application.Users.Commands.CreateUser;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;

namespace WebApp.Pages.Roles
{
    public class CreateModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public CreateModel(ILogger<CreateModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [BindProperty]
        public CreateUserRoleCommand NewRole { get; set; }
        public void OnGet()
        {
            NewRole = new();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidationResult validationCheck = new CreateUserRoleCommandValidator().Validate(NewRole);
            validationCheck.AddToModelState(ModelState, nameof(NewRole));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            IdentityResult result = await _mediator.Send(NewRole);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Created new role name {NewRole.Name}");
                return RedirectToPage($"./{nameof(Index)}").WithSuccess($"Created new role {NewRole.Name}");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();

        }
    }
}
