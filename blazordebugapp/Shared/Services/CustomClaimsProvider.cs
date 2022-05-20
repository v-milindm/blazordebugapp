using blazordebugapp.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using System.Security.Claims;

namespace blazordebugapp.Shared.Services
{
    public class CustomClaimsProvider : IClaimsTransformation
    {
        private readonly IUserManagerRepository userRepo;
        private ClaimsPrincipal currentuser { get; set; }

        public CustomClaimsProvider(IUserManagerRepository userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            try
            {
                if (currentuser == null)
                {
                    currentuser = await this.userRepo.GetAuthUser();

                    foreach (var currentClaim in currentuser.Claims)
                    {
                        var claimType = currentClaim.Type;
                        var claimValue = currentClaim.Value;

                        if (!principal.HasClaim(claim => claim.Type == claimType && claim.Value == claimValue))
                        {
                            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

                            claimsIdentity.AddClaim(new Claim(claimType, claimValue));

                            principal.AddIdentity(claimsIdentity);
                        }
                    }
                }

                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception at CustomClaimsProvider.TransformAsync, message: {0}", ex.Message);
                Debug.WriteLine("Exception at CustomClaimsProvider.TransformAsync, message: {0}", ex.Message);
                throw;
            }
        }
    }
}
