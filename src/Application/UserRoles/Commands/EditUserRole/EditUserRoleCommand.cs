using Application.UserRoles.Queries.GetUserRoles;
using AutoMapper;
using MediatR;
using static Application.Common.Mappings.MappingProfile;

namespace Application.UserRoles.Commands.EditUserRole;

public class EditUserRoleCommand : IRequest<List<string>>, IMapFrom<RoleDTO>
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RoleDTO, EditUserRoleCommand>();
    }
}
