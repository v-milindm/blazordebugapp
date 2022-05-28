using blazordebugapp.Shared.Interfaces;
using blazordebugapp.Shared.Models;
using blazordebugapp.Shared.Routes;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Claims;

namespace blazordebugapp.Client.Services
{
    public class UserRoleService : IUserManagerRepository
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public UserRoleService(HttpClient httpClient, ILogger<UserRoleService> logger)
        {
            this.httpClient = httpClient;
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
                var serverResponse = await this.httpClient.GetAsync(ServerRoutes.AdminApi.GetUser);
                String responseContent = await serverResponse.Content.ReadAsStringAsync();

                if (serverResponse.IsSuccessStatusCode && serverResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var retServerAuthUser = Newtonsoft.Json.JsonConvert.DeserializeObject<UserIdentityClaims>(responseContent);

                    if (retServerAuthUser == null)
                    {
                        throw new ArgumentNullException(nameof(retServerAuthUser), "Current user returned null from server.");
                    }

                    var claims = new[] { new Claim(ClaimTypes.Name, retServerAuthUser.UserName) }
                    .Concat(retServerAuthUser.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)));

                    var identity = new ClaimsIdentity(claims, "Server authentication");
                    var user = new ClaimsPrincipal(identity);

                    return user;
                }
                else
                {
                    throw new InvalidOperationException($"Response indicates that server did not return the authenticated user information. Error Message: {responseContent}");
                }
            }
            catch(Exception ex)
            {
                string errorMessage = string.Format("Exception at UserRoleService.GetAuthUser, message: {0}, stack: {1}", ex.Message, ex.StackTrace);

                Console.WriteLine(errorMessage);
                Debug.WriteLine(errorMessage);
                logger.LogError(errorMessage);
                throw;
            }
        }

        public Task<RoleBasedAccessModel> UpdateUserRolesAccessAsync(RoleBasedAccessModel updateUser)
        {
            throw new NotImplementedException();
        }
    }
}
