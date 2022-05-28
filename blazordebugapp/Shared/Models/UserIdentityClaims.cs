using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Models
{
    /// <summary>
    /// Helper class to serialize over HTTP to client
    /// </summary>
    /// <remarks>There was error in ClaimsPrincipal object serialization</remarks>
    public class UserIdentityClaims
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public List<ClaimsData> Claims { get; set; }
    }

    public class ClaimsData
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
