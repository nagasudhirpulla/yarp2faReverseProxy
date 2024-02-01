using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UserRoles.Queries.GetUserRoles;

public class GetUserRolesQuery : IRequest<List<RoleDTO>>
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<RoleDTO>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetUserRolesQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleDTO>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            List<RoleDTO> roles = await _roleManager.Roles.Select(r => new RoleDTO { Id = r.Id, Name = r.Name }).ToListAsync();

            return roles;
        }
    }
}