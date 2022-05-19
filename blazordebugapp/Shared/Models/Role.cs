using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace blazordebugapp.Shared.Models
{
    public class Role
    {
        [JsonPropertyName("roleid")]
        [JsonProperty("roleid")]
        public Guid RoleId { get; set; }

        [JsonPropertyName("rolename")]
        [JsonProperty("rolename")]
        public string RoleName { get; set; }
    }
}
