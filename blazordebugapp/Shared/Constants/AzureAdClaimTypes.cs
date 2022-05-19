using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Constants
{
    public static class AzureAdClaimTypes
    {
        public const string ObjectId = "https://schemas.microsoft.com/identity/claims/objectidentifier";

        public const string UserName = "name";

        public const string Email = "preferred_username";

        public const string AccessToken = "access_token";
    }
}
