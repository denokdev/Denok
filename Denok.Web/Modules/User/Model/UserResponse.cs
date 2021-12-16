using System;
using System.Text.Json.Serialization;

namespace Denok.Web.Modules.User.Model
{
    public class UserResponse : Base.Model.BaseResponseModel
    {

        public UserResponse() {}

        public UserResponse(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;

            CreatedAt = user.CreatedAt;
            CreatedBy = user.CreatedBy;
            UpdatedAt = user.UpdatedAt;
            DeletedAt = user.DeletedAt;
            IsDeleted = user.IsDeleted;
        }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}