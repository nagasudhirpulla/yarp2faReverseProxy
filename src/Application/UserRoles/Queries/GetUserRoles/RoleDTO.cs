using AutoMapper;
using Microsoft.AspNetCore.Identity;
using static Application.Common.Mappings.MappingProfile;

namespace Application.UserRoles.Queries.GetUserRoles;

public class RoleDTO : IMapFrom<IdentityRole>
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<IdentityRole, RoleDTO>();
    }
}
