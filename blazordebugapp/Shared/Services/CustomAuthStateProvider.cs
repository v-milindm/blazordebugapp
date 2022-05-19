using blazordebugapp.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IUserManagerRepository userRepo;
        ClaimsPrincipal currentuser = null;
        public CustomAuthStateProvider(IUserManagerRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        /// <summary>
        /// For the Blazor client, we go out to the back end server to get
        /// the authenticated user info.
        /// This avoids doing Server auth and client auth (double auth)
        /// </summary>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (currentuser == null)
                {
                    currentuser = await this.userRepo.GetAuthUser();
                }

                return (new AuthenticationState(currentuser));
            }
            catch (Exception)
            {
                // log error details if needed
            }

            return null;
        }
    }
}
