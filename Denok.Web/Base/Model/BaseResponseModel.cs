using System;
using System.Text.Json.Serialization;

namespace Denok.Web.Base.Model
{

    public class BaseResponseModel
    {   
        private DateTime _createdAt;

        private DateTime _updatedAt;

        private DateTime _deletedAt;

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt 
        { 
            get => Utils.Utils.ConvertDateTimeToLocalTimeZone(_createdAt); 
            set => _createdAt = value.Value; 
        }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt 
        { 
            get => Utils.Utils.ConvertDateTimeToLocalTimeZone(_updatedAt); 
            set => _updatedAt = value.Value; 
        }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("deletedAt")]
        public DateTime? DeletedAt 
        {
            get => Utils.Utils.ConvertDateTimeToLocalTimeZone(_deletedAt);
            set => _deletedAt = value.Value; 
        }

        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}