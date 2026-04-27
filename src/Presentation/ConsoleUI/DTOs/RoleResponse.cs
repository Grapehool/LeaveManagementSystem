using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ConsoleUI.DTOs
{
    public class RoleResponse
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
    }
}
