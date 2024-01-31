using AutoMapper;
using MediatR;
using Application.Users.Queries.GetAppUsers;
using Core.Entities;
using static Application.Common.Mappings.MappingProfile;

namespace Application.Users.Commands.EditUser;

public class EditUserCommand : IRequest<List<string>>, IMapFrom<UserDTO>
{
    public string Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    //public string UserRole { get; set; } = SecurityConstants.GuestRoleString;
    public bool IsTwoFactorEnabled { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApplicationUser, EditUserCommand>()
            .ForMember(d => d.Username, opt => opt.MapFrom(s => s.UserName))
            .ForMember(d => d.IsTwoFactorEnabled, opt => opt.MapFrom(s => s.TwoFactorEnabled));
    }
}
