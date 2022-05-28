using blazordebugapp.Shared.Constants;
using blazordebugapp.Shared.Interfaces;
using blazordebugapp.Shared.Models;
using blazordebugapp.Shared.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Diagnostics;

namespace blazordebugapp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route(ServerRoutes.Base)]
    public class AdminController : Controller
    {
        private readonly IUserManagerRepository userManagerRepo;
        private readonly IIdentityService userIdentity;
        private readonly GraphServiceClient graphServiceClient;
        private readonly ILogger logger;

        public AdminController(IUserManagerRepository userManagerRepo,
            IIdentityService userRepo,
            GraphServiceClient graphServiceClient,
            ILogger<AdminController> logger)
        {
            this.userManagerRepo = userManagerRepo;
            this.userIdentity = userRepo;
            this.graphServiceClient = graphServiceClient;
            this.logger = logger;
        }

        /// <summary>
        /// Gets a list of roles assigned to a user.
        /// </summary>
        /// <param name="userName">The email address of an authenticated user.</param>
        /// <returns>A <see cref="string[]"/> of role names, which may be empty.</returns>
        /// <remarks>Used for view access.</remarks>
        [Authorize]
        [HttpGet(ServerRoutes.AdminApi.GetAuthUser)]
        public UserIdentityClaims GetAuthUser()
        {
            try
            {
                var newUser = new UserIdentityClaims
                {
                    IsAuthenticated = this.userIdentity.IsAuthenticated(),
                    UserName = this.userIdentity.GetName(),
                    Claims = new()
                };

                if(newUser == null || !newUser.IsAuthenticated)
                {
                    throw new InvalidOperationException("Current user returned null data or was not authenticated from server.");
                }

                foreach (var claim in this.userIdentity.UserPrincipal.Claims)
                {
                    newUser.Claims.Add(new() { ClaimType = claim.Type, ClaimValue = claim.Value });
                }

                return newUser;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Exception at AdminController.GetAuthUser, message: {0}, stack: {1}", ex.Message, ex.StackTrace);

                Console.WriteLine(errorMessage);
                Debug.WriteLine(errorMessage);
                logger.LogError(errorMessage);
                throw;
            }
        }

        [Authorize(Roles = RoleTypes.Owner)]
        [HttpGet]
        [Route(ServerRoutes.AdminApi.GetManager)]
        public async Task<ActionResult> GetManagerDetails()
        {
            try
            {
                var meManager = await graphServiceClient.Me.Manager.Request().GetAsync() as User;
                var me = await graphServiceClient.Me.Request().GetAsync();

                var graphUser = new
                {
                    UserFirstName = me.GivenName,
                    UserLastName = me.Surname,
                    UserDisplayName = me.DisplayName,
                    UserAlias = me.UserPrincipalName,
                    ManagerFirstName = meManager.GivenName,
                    ManagerLastName = meManager.Surname,
                    ManagerDisplayName = meManager.DisplayName,
                    ManagerAlias = meManager.UserPrincipalName
                };

                return Ok(graphUser);
            }
            catch (Microsoft.Graph.ServiceException graphEx)
            {
                // NOTEs:  If there is an issue getting the ME data we will
                // just return an empty GraphUserModel with no error for ui to act on
                // if we get challenge exception, then we want an error
                // message to be show in ui HasError = true;

                var graphErrorUser = new
                {
                    HasError = false,
                    ErrorMessage = $"{graphEx.Message} {graphEx.InnerException} {graphEx.StackTrace}"
                };

                if (graphEx.InnerException is MicrosoftIdentityWebChallengeUserException)
                {
                    graphErrorUser = new
                    {
                        HasError = true,
                        ErrorMessage = $"{graphEx.Message} {graphEx.InnerException} {graphEx.StackTrace}"
                    };
                }

                return Ok(graphErrorUser);
            }
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
