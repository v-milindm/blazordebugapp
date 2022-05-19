using blazordebugapp.Shared.Interfaces;
using blazordebugapp.Shared.Models;
using blazordebugapp.Shared.Routes;
using System.Net.Http.Json;
using System.Security.Claims;

namespace blazordebugapp.Client.Services
{
    public class UserRoleService : IUserManagerRepository
    {
        private readonly HttpClient httpClient;

        public UserRoleService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
            throw new NotImplementedException();
        }

        public Task<List<RoleBasedAccessModel>> GetAllAccessUsersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ClaimsPrincipal> GetAuthUser()
        {
            try
            {
                var retServerAuthUser = await this.httpClient.GetFromJsonAsync<UserIdentityClaims>(ServerRoutes.AdminApi.GetUser);

                var claims = new[] { new Claim(ClaimTypes.Name, retServerAuthUser.UserName) }
                .Concat(retServerAuthUser.Claims.Select(c => new Claim(c.Item1, c.Item2)));

                var identity = new ClaimsIdentity(claims, "Server authentication");
                var user = new ClaimsPrincipal(identity);

                return user;
            }
            catch
            {
                // log error details if needed
            }

            return null;
        }

        public Task<RoleBasedAccessModel> UpdateUserRolesAccessAsync(RoleBasedAccessModel updateUser)
        {
            throw new NotImplementedException();
        }
    }
}
