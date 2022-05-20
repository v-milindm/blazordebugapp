using blazordebugapp.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using System.Security.Claims;

namespace blazordebugapp.Client.Authorization
{
    public class CustomAuthorizeView : AuthorizeViewCore
    {
        private AuthenticationState? localAuthenticationState;
        // private bool? isAuthorized;
        private readonly IAuthorizeData[] selfAsAuthorizeData;

        /// <summary>
        /// Constructs an instance of <see cref="AuthorizeView"/>.
        /// </summary>
        public CustomAuthorizeView()
        {
            selfAsAuthorizeData = new[] { new AuthorizeData(this.Policy, this.Roles) };
        }

        /// <summary>
        /// The policy name that determines whether the content can be displayed.
        /// </summary>
        [Parameter] public string? Policy { get; set; }

        /// <summary>
        /// A comma delimited list of roles that are allowed to display the content.
        /// </summary>
        [Parameter] public string? Roles { get; set; }

        [CascadingParameter] private Task<AuthenticationState>? CustomAuthenticationState { get; set; }

        /// <inheritdoc />
        protected override async Task OnParametersSetAsync()
        {
            if(CustomAuthenticationState == null)
            {
                Console.WriteLine("OnParametersSetAsync CustomAuthenticationState is null: {0}", CustomAuthenticationState is null);
                Console.WriteLine("OnParametersSetAsync CustomAuthenticationState is null: {0}", CustomAuthenticationState is null);
                throw new ArgumentNullException(nameof(CustomAuthenticationState));
            }

            localAuthenticationState = await CustomAuthenticationState;
            Console.WriteLine("OnParametersSetAsync currentAuthenticationState: {0}", localAuthenticationState.User is not null);

            foreach(var claim in localAuthenticationState.User.Claims)
            {
                Console.WriteLine("OnParametersSetAsync claim: {0}", claim.Type);
                Console.WriteLine("OnParametersSetAsync claim: {0}", claim.Value);
            }

            await base.OnParametersSetAsync();
        }

        /// <summary>
        /// Gets the data used for authorization.
        /// </summary>
        protected override IAuthorizeData[] GetAuthorizeData()
            => selfAsAuthorizeData;
    }
}
