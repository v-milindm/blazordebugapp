using blazordebugapp.Client;
using blazordebugapp.Client.Services;
using blazordebugapp.Shared.Interfaces;
using blazordebugapp.Shared.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace blazordebugapp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            // builder.RootComponents.Add<App>("#app");
            // builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            //builder.Services.AddHttpClient("blazordebugapp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            //    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            //builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("blazordebugapp.ServerAPI"));

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);

                options.ProviderOptions.LoginMode = "redirect";
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.ReadBasic.All");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("api://api.id.uri/access_as_user");
            });

            builder.Services.AddScoped<IIdentityService, AzureAdIdentityService>();
            builder.Services.AddTransient<IUserManagerRepository, UserRoleService>();
            ConfigureCommonServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureCommonServices(IServiceCollection services)
        {
            // services.AddTransient<IClaimsTransformation, CustomClaimsProvider>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        }
    }
}