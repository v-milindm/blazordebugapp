using blazordebugapp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Interfaces
{
    /// <summary>
    /// Manage users and user roles
    /// </summary>
    public interface IUserManagerRepository
    {
        /// <summary>
        /// Gets all Roll Access Users
        /// </summary>>
        /// <returns><see cref="List<RoleBasedAccessModel></RoleBasedAccessModel>"/> or null</returns>
        Task<List<RoleBasedAccessModel>> GetAllAccessUsersAsync();

        /// <summary>
        /// Tries to get a user access policies by email
        /// </summary>
        /// <param name="userEmail">unique identifier of the user; in this case email</param>
        /// <returns><see cref="RoleBasedAccessModel"/> or null</returns>
        Task<RoleBasedAccessModel> GetAccessInformationByEmailAsync(string userEmail);

        /// <summary>
        /// Adds a new user with roles to the db
        /// </summary>
        /// <param name="newUser">A new user role information object</param>
        /// <returns><see cref="RoleBasedAccessModel"/></returns>
        Task<RoleBasedAccessModel> AddNewUserAccessAsync(RoleBasedAccessModel newUser);

        /// <summary>
        /// Update a user roles
        /// </summary>
        /// <param name="updateUser"></param>
        /// <returns><see cref="RoleBasedAccessModel"/></returns>
        Task<RoleBasedAccessModel> UpdateUserRolesAccessAsync(RoleBasedAccessModel updateUser);

        /// <summary>
        /// Delete user access
        /// </summary>
        /// <param name="deleteUser"></param>
        /// <returns>boolean</returns>
        Task<Boolean> DeleteUserAsync(RoleBasedAccessModel deleteUser);

        /// <summary>
        /// Gets the authenticated user from the server
        /// </summary>
        /// <returns></returns>
        Task<ClaimsPrincipal> GetAuthUser();
    }
}
