using System.Net.Http.Headers;
using System.Security.Claims;
using blazordebugapp.Shared.Interfaces;
using blazordebugapp.Shared.Repository;
using blazordebugapp.Shared.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace Company.WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

            // Add services to the container.
            // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            // Authentication services
            var initialScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');

            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApp(o =>
               {
                   builder.Configuration.GetSection("AzureAd").Bind(o);
                   o.Events ??= new OpenIdConnectEvents();
                   o.Events.OnTokenValidated += (tokenContext) => OnTokenValidatedFunc(tokenContext, builder);
               }).EnableTokenAcquisitionToCallDownstreamApi(o =>
               builder.Configuration.GetSection("AzureAd").Bind(o), initialScopes)
                       .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
                       .AddDistributedTokenCaches();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });

            // Add consent handler
            builder.Services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

            // user claim / identity helper services
            builder.Services.AddScoped<IIdentityService, AzureAdIdentityService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            builder.Services.AddTransient<IUserManagerRepository, UserManagerRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapRazorPages();
            app.MapControllers();
            // app.MapFallbackToFile("index.html");
            app.MapFallbackToPage("/_Host");

            app.Run();
        }

        private static async Task OnTokenValidatedFunc(TokenValidatedContext context, WebApplicationBuilder builder)
        {
            if (context.Principal.Identity.IsAuthenticated)
            {
                // authentication services
                var graphScopes = builder.Configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');

                var tokenAcquisition = context.HttpContext.RequestServices
             .GetRequiredService<ITokenAcquisition>();

                var graphClient = new GraphServiceClient(
                    new DelegateAuthenticationProvider(async (request) =>
                    {
                        var token = await tokenAcquisition
                            .GetAccessTokenForUserAsync(graphScopes, user: context.Principal);
                        request.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", token);
                    })
                );

                // create a new custom identity item (it is not getting populated via DI)
                AzureAdIdentityService identity = new AzureAdIdentityService(context.Principal);

                var serviceProvider = builder.Services.BuildServiceProvider();
                var userRolesService = serviceProvider.GetService<IUserManagerRepository>();

                // find the user in the admin roles container
                var userRoleData = await userRolesService.GetAccessInformationByEmailAsync(identity.GetEmail());

                if (userRoleData?.Roles?.Count > 0)
                {
                    // build the role claims
                    foreach (var role in userRoleData.Roles)
                    {
                        ((ClaimsIdentity)identity.UserPrincipal.Identity)
                            .AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
                    }
                }
            }

            // Custom code here
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}