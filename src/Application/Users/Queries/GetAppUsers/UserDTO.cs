﻿using AutoMapper;
using System;
using System.Text;
using Core.Entities;
using static Application.Common.Mappings.MappingProfile;

namespace Application.Users.Queries.GetAppUsers
{
    public class UserDTO : IMapFrom<ApplicationUser>
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public bool IsTwoFactorEnabled { get; set; } = false;
        public string PhoneNumber { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ApplicationUser, UserDTO>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Username, opt => opt.MapFrom(s => s.UserName))
                .ForMember(d => d.IsTwoFactorEnabled, opt => opt.MapFrom(s => s.TwoFactorEnabled));
        }
    }
}
