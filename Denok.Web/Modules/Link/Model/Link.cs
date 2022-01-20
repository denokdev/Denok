using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Denok.Web.Modules.Link.Model
{

    public class Link : Base.Model.BaseModel
    {
        public Link()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        } 

        public Link(string originalLink, string generatedLink)
        {
            OriginalLink = originalLink;
            GeneratedLink = generatedLink;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }  

        public Link(string originalLink, string generatedLink, string createdBy)
        {
            OriginalLink = originalLink;
            GeneratedLink = generatedLink;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
        } 

        [BsonElement("originalLink")]
        public string OriginalLink { get; set; }

        [BsonElement("generatedLink")]
        public string GeneratedLink { get; set; }

        [BsonElement("totalVisits")]
        public long TotalVisits { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}