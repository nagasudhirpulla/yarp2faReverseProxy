using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;
using Application.UserRoles.Queries.GetUserRoles;
using Application.UserRoles.Queries.GetUserRoleById;
using Application.UserRoles.Commands.DeleteUserRole;

namespace WebApp.Pages.Roles;

[Authorize(Roles = SecurityConstants.AdminRoleString)]
public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly IMediator _mediator;

    public DeleteModel(ILogger<DeleteModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [BindProperty]
    public RoleDTO DRole { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        DRole = await _mediator.Send(new GetUserRoleByIdQuery() { Id = id });

        if (DRole == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        List<string> errs = await _mediator.Send(new DeleteUserRoleCommand() { Id = DRole.Id });

        if (errs.Count == 0)
        {
            _logger.LogInformation("Role deleted successfully");
            return RedirectToPage($"./{nameof(Index)}").WithSuccess("Role deletion done");
        }

        foreach (var error in errs)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        return Page();
    }
}
