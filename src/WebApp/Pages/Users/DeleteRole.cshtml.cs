using Application.Common;
using Application.UserRoles.Queries.GetUserRoles;
using Application.Users.Commands.DeleteRoleFromUser;
using Application.Users.Queries.GetAppUsers;
using Application.Users.Queries.GetUserById;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Extensions;

namespace WebApp.Pages.Users;
public class DeleteRoleModel(ILogger<DeleteRoleModel> logger, IMediator mediator, ICurrentUserService currentUserService) : PageModel
{
    private readonly ILogger<DeleteRoleModel> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    [BindProperty]
    public DeleteRoleFromUserCommand DeleteRoleCmd { get; set; }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    public SelectList URoles { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        UserDTO usr = await _mediator.Send(new GetUserByIdQuery() { Id = id });
        if (usr == null)
        {
            return NotFound();
        }
        Username = usr.Username;

        DeleteRoleCmd = new DeleteRoleFromUserCommand() { UserId = id };

        await InitSelectListItems();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await InitSelectListItems();

        ValidationResult validationCheck = new DeleteRoleFromUserCommandValidator().Validate(DeleteRoleCmd);
        validationCheck.AddToModelState(ModelState, nameof(DeleteRoleCmd));

        if (!ModelState.IsValid)
        {
            return Page();
        }

        IdentityResult res = await _mediator.Send(DeleteRoleCmd);

        // check if we have any errors and redirect if successful
        if (res.Succeeded)
        {
            _logger.LogInformation("User role delete operation successful");
            return RedirectToPage($"./Details", new { id = DeleteRoleCmd.UserId }).WithSuccess("User role delete done");
        }

        foreach (var error in res.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }

        return Page();
    }

    public async Task InitSelectListItems()
    {
        // get all the roles
        List<RoleDTO> roles = await _mediator.Send(new GetUserRolesQuery());
        URoles = new SelectList(roles.Select(r => r.Name));
    }
}

