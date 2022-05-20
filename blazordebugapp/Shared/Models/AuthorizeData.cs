using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Models
{
    public class AuthorizeData : IAuthorizeData
    {
        private readonly string _policy;
        private readonly string _roles;

        public AuthorizeData(string policy, string roles)
        {
            _policy = policy;
            _roles = roles;
        }

        public string? Policy
        {
            get => _policy;
            set => throw new NotSupportedException();
        }

        public string? Roles
        {
            get => _roles;
            set => throw new NotSupportedException();
        }

        // AuthorizeView doesn't expose any such parameter, as it wouldn't be used anyway,
        // since we already have the ClaimsPrincipal by the time AuthorizeView gets involved.
        public string? AuthenticationSchemes
        {
            get => null;
            set => throw new NotSupportedException();
        }
    }
}
