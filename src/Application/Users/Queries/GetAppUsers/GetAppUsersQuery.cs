using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Application.Users.Queries.GetAppUsers;

public class GetAppUsersQuery : IRequest<UserListVM>
{
    public class GetAppUsersQueryHandler : IRequestHandler<GetAppUsersQuery, UserListVM>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityInit _identityInit;
        private readonly IMapper _mapper;

        public GetAppUsersQueryHandler(UserManager<ApplicationUser> userManager, IdentityInit identityInit, IMapper mapper)
        {
            _userManager = userManager;
            _identityInit = identityInit;
            _mapper = mapper;
        }

        public async Task<UserListVM> Handle(GetAppUsersQuery request, CancellationToken cancellationToken)
        {
            UserListVM vm = new()
            {
                Users = new List<UserDTO>()
            };
            // get the list of users
            List<ApplicationUser> users = await _userManager.Users.OrderBy(u => u.UserName)
                                                                    .ToListAsync();
            foreach (ApplicationUser user in users)
            {
                // get user is of admin role
                //bool isSuperAdmin = (await _userManager.GetRolesAsync(user)).Any(r => r == SecurityConstants.AdminRoleString);
                bool isSuperAdmin = (user.UserName == _identityInit.AdminUserName);
                if (!isSuperAdmin)
                {
                    // add user to vm only if not admin
                    IList<string> existingRoles = await _userManager.GetRolesAsync(user);
                    UserDTO uDTO = _mapper.Map<UserDTO>(user);
                    uDTO.UserRoles = existingRoles.ToList();
                    vm.Users.Add(uDTO);
                }

            }
            return vm;
        }
    }
}
