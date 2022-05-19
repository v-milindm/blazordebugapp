using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Models
{
    public class RoleBasedAccessModel
    {
        public RoleBasedAccessModel()
        {
            Roles = new List<Role>();
        }

        /// <summary>
		/// The primary Cosmos DB key.
		/// </summary>
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        [Required]
        public Guid Id { get; set; }

        [JsonPropertyName("useremail")]
        [JsonProperty("useremail")]
        [Required]
        public string UserEmail { get; set; }

        [JsonPropertyName("userfullname")]
        [JsonProperty("userfullname")]
        [Required]
        public string UserFullName { get; set; }

        [JsonPropertyName("roles")]
        [JsonProperty("roles")]
        public IList<Role> Roles { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string RolesRender
        {
            get
            {
                string retString = string.Empty;
                foreach (var item in Roles)
                {
                    retString += $" {item.RoleName} |";
                }
                return retString.TrimEnd('|');
            }
        }

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
