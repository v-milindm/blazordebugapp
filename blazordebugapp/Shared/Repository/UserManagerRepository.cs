using blazordebugapp.Shared.Constants;
using blazordebugapp.Shared.Interfaces;
using blazordebugapp.Shared.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Repository
{
    /// <summary>
    /// Manage users and user roles
    /// </summary>
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly IIdentityService identityService;

        public UserManagerRepository(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public Task<RoleBasedAccessModel> AddNewUserAccessAsync(RoleBasedAccessModel newUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(RoleBasedAccessModel deleteUser)
        {
            throw new NotImplementedException();
        }

        public Task<RoleBasedAccessModel> GetAccessInformationByEmailAsync(string userEmail)
        {
            RoleBasedAccessModel userRoleInfo = new RoleBasedAccessModel()
            {
                Id = Guid.NewGuid(),
                Roles = new List<Role>() {
                    new Role(){ RoleId = Guid.NewGuid(), RoleName = RoleTypes.Admin },
                    new Role(){ RoleId = Guid.NewGuid(), RoleName = RoleTypes.Owner }
                },
                UserEmail = userEmail,
                UserFullName = "full username"
            };

            return Task.FromResult(userRoleInfo);
        }

        public Task<List<RoleBasedAccessModel>> GetAllAccessUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ClaimsPrincipal> GetAuthUser()
        {
            var currentUser = this.identityService.UserPrincipal;
            var userRoleData = await this.GetAccessInformationByEmailAsync(this.identityService.GetEmail());
            userRoleData.UserFullName = identityService.GetName();

            if (userRoleData?.Roles?.Count > 0)
            {
                // build the role claims
                foreach (var role in userRoleData.Roles)
                {
                    ((ClaimsIdentity)currentUser.Identity)
                        .AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
                }
            }

            return currentUser;
        }

        public Task<RoleBasedAccessModel> UpdateUserRolesAccessAsync(RoleBasedAccessModel updateUser)
        {
            throw new NotImplementedException();
        }
    }
}
