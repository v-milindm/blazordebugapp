using blazordebugapp.Shared.Constants;
using blazordebugapp.Shared.Interfaces;
using blazordebugapp.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public UserManagerRepository(IIdentityService identityService, ILogger<UserManagerRepository> logger)
        {
            this.identityService = identityService;
            this.logger = logger;
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
            
            if(currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser), "Current user returned null from Identity Service at UserManagerRepository.GetAuthUser() method.");
            }

            var userRoleData = await this.GetAccessInformationByEmailAsync(this.identityService.GetEmail());
            userRoleData.UserFullName = identityService.GetName();

            if (userRoleData?.Roles?.Count > 0)
            {
                // build the role claims
                foreach (var role in userRoleData.Roles)
                {
                    ClaimsIdentity principal = (ClaimsIdentity)currentUser.Identity;

                    var claimType = ClaimTypes.Role;
                    var claimValue = role.RoleName;

                    if (!principal.HasClaim(claim => claim.Type == claimType && claim.Value == claimValue))
                    {
                        principal.AddClaim(new Claim(claimType, claimValue));
                    }
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
