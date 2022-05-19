using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Routes
{
    public static class ServerRoutes
    {
        public const string Root = "server";
        public const string Base = Root + "/[controller]";

        public static class AdminApi
        {
            // For controller method paths
            public const string Base = Root + "/admin";
            public const string GetAuthUser = "authuser";
            public const string GetManager = "manager";

            // For client end-points
            public const string GetUser = Base + "/authuser";
            public const string GetManagerUri = Base + "/manager";
        }
    }
}
