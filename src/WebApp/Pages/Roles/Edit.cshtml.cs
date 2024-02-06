using Application.Users;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Extensions;
using Application.UserRoles.Queries.GetUserRoleById;
using Application.UserRoles.Queries.GetUserRoles;
using FluentValidation.Results;
using FluentValidation.AspNetCore;
using Application.UserRoles.Commands.EditUserRole;

namespace WebApp.Pages.Roles;

[Authorize(Roles = SecurityConstants.AdminRoleString)]
public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public EditModel(ILogger<EditModel> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    [BindProperty]
    public EditUserRoleCommand UserRole { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        RoleDTO? role = await _mediator.Send(new GetUserRoleByIdQuery() { Id = id });
        if (role == null)
        {
            return NotFound();
        }
        UserRole = _mapper.Map<EditUserRoleCommand>(role);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ValidationResult validationCheck = new EditUserRoleCommandValidator().Validate(UserRole);
        validationCheck.AddToModelState(ModelState, nameof(UserRole));

        if (!ModelState.IsValid)
        {
            return Page();
        }

        List<string> errors = await _mediator.Send(UserRole);

        foreach (var error in errors)
        {
            ModelState.AddModelError(string.Empty, error);
        }

        // check if we have any errors and redirect if successful
        if (errors.Count == 0)
        {
            _logger.LogInformation("Role edit operation successful");
            return RedirectToPage($"./{nameof(Index)}").WithSuccess("Role Editing done");
        }

        return Page();
    }

}