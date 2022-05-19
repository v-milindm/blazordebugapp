using blazordebugapp.Shared.Constants;
using blazordebugapp.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Services
{
    /// <summary>
    /// Helper to get authenticated user information
    /// </summary>
    public class AzureAdIdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AzureAdIdentityService()
        {
        }

        public AzureAdIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            UserPrincipal = httpContextAccessor.HttpContext.User;
        }

        public AzureAdIdentityService(ClaimsPrincipal claimsPrincipal)
        {
            UserPrincipal = claimsPrincipal;
        }

        public ClaimsPrincipal UserPrincipal { get; }

        public bool IsAuthenticated()
        {
            return UserPrincipal.Identity.IsAuthenticated;
        }

        public string GetEmail()
        {
            return UserPrincipal.Claims
               .FirstOrDefault(c => c.Type == AzureAdClaimTypes.Email)?.Value;
        }

        public string GetName()
        {
            return UserPrincipal.Claims
               .FirstOrDefault(c => c.Type == AzureAdClaimTypes.UserName)?.Value;
        }

        public string GetId()
        {
            return UserPrincipal.Claims
                .FirstOrDefault(c => c.Type == AzureAdClaimTypes.ObjectId)?.Value;
        }

    }
}
