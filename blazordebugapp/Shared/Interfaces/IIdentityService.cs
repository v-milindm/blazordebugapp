using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Interfaces
{
    /// <summary>
    /// Interface to expose authenticated user information
    /// </summary>
    /// <remarks>Used for DI</remarks>
    public interface IIdentityService
    {
        bool IsAuthenticated();

        string GetEmail();

        string GetId();

        string GetName();

        ClaimsPrincipal UserPrincipal { get; }
    }
}
