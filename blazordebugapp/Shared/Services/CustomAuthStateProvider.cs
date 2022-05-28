using blazordebugapp.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IUserManagerRepository userRepo;
        private readonly ILogger logger;
        ClaimsPrincipal currentuser = null;
        public CustomAuthStateProvider(IUserManagerRepository userRepo, ILogger<CustomAuthStateProvider> logger)
        {
            this.userRepo = userRepo;
            this.logger = logger;
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

                    if (currentuser == null)
                    {
                        throw new ArgumentNullException(nameof(currentuser), "Current user returned null from user service.");
                    }
                }

                return (new AuthenticationState(currentuser));
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Exception at CustomAuthStateProvider.GetAuthenticationStateAsync, message: {0}, stack: {1}", ex.Message, ex.StackTrace);

                Console.WriteLine(errorMessage);
                Debug.WriteLine(errorMessage);
                logger.LogError(errorMessage);
                throw;
            }
        }
    }
}
