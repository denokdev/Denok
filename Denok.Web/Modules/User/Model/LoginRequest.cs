using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Denok.Web.Modules.User.Model
{

    public class LoginRequest
    {
        [Required(ErrorMessage = "username is required")]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "password is required")]
        [JsonPropertyName("password")]
        public string Password { get; set; }   
    }
}