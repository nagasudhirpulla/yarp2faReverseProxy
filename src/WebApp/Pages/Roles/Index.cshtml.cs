using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.UserRoles.Queries.GetUserRoles;

namespace WebApp.Pages.Roles;

[Authorize(Roles = SecurityConstants.AdminRoleString)]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;
    public IList<RoleDTO> UserRoles { get; set; }

    public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task OnGetAsync()
    {
        UserRoles = await _mediator.Send(new GetUserRolesQuery());
    }
}
