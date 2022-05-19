using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Constants
{
    /// <summary>
    /// Static class to hold application role names
    /// </summary>
    public static class RoleTypes
    {
        /// <summary>
        /// Big Khahuna user that has all access
        /// </summary>
        public const string Owner = "Owner";

        /// <summary>
        /// Admin user that can perform elevated tasks
        /// </summary>
        public const string Admin = "Administrator";

        /// <summary>
        /// Plain jane user
        /// </summary>
        public const string User = "User";
    }
}
