using Application.UserRoles.Queries.GetUserRoles;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UserRoles.Queries.GetUserRoleById;

public class GetUserRoleByIdQuery : IRequest<RoleDTO?>
{
    public string Id { get; set; } = string.Empty;
    public class GetUserRoleByIdQueryHandler : IRequestHandler<GetUserRoleByIdQuery, RoleDTO?>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public GetUserRoleByIdQueryHandler(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<RoleDTO?> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == null)
            {
                return null;
            }

            IdentityRole? role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken: cancellationToken);
            if (role == null)
            {
                return null;
            }

            RoleDTO roleDto = _mapper.Map<RoleDTO>(role);
            return roleDto;
        }
    }
}
