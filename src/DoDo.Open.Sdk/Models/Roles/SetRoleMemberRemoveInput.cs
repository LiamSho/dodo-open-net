﻿using System.Text.Json.Serialization;

namespace DoDo.Open.Sdk.Models.Roles
{
    public class SetRoleMemberRemoveInput
    {
        /// <summary>
        /// 群号
        /// </summary>
        [JsonPropertyName("islandId")]
        public string IslandId { get; set; }

        /// <summary>
        /// DoDo号
        /// </summary>
        [JsonPropertyName("dodoId")]
        public string DodoId { get; set; }

        /// <summary>
        /// 身份组ID
        /// </summary>
        [JsonPropertyName("roleId")]
        public string RoleId { get; set; }
    }
}
