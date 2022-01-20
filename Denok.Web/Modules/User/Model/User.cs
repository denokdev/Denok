using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Denok.Web.Modules.User.Model
{
    public class User : Base.Model.BaseModel 
    {

        public User()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public User(string username, string email, string phoneNumber, string password)
        {
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("profilePicture")]
        public string ProfilePicture { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("deviceToken")]
        public string DeviceToken { get; set; }

        [BsonElement("otpSecret")]
        public string OtpSecret { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        public bool IsValidPassword(string password)
        {
            return Lib.PasswordHasher.Pbkdf2.ValidatePassword(password, Password);
        }
    }
}