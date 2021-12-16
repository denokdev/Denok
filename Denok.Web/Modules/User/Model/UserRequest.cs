using System;
using System.Text.Json.Serialization;

namespace Denok.Web.Modules.User.Model
{
    public class UserRequest 
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        public User ToUserModel()
        {
            var user = new User(Username, 
                Email, PhoneNumber, Lib.PasswordHasher.Pbkdf2.HashPassword(Password));
            return user;
        }
    }
}