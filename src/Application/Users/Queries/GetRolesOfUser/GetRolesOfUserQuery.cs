using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.GetRolesOfUser;

public class GetRolesOfUserQuery : IRequest<List<string>>
{
    public string? Id { get; set; }
    public class GetRolesOfUserQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<GetRolesOfUserQuery, List<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<List<string>> Handle(GetRolesOfUserQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                return [];
            }
            ApplicationUser? user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return [];
            }

            List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();

            return roles;
        }
    }
}

