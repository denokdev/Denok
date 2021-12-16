using System;
using System.Text.Json.Serialization;

namespace Denok.Web.Modules.Link.Model
{
    public class LinkRequest 
    {
        [JsonPropertyName("originalLink")]
        public string OriginalLink { get; set; }

        [JsonPropertyName("generatedLink")]
        public string GeneratedLink { get; set; }

        [JsonPropertyName("totalVisits")]
        public long TotalVisits { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public Link ToLinkModel()
        {
            var link = new Link(OriginalLink, GeneratedLink);
            link.Description = Description;
            link.CreatedBy = CreatedBy;
            return link;
        }
    }
}