using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Denok.Web.Base.Model
{
    public class BaseModel
    {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; }
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("deletedAt")]
        public DateTime DeletedAt { get; set; }

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}