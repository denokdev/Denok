using System;
using System.Text.Json.Serialization;

namespace Denok.Web.Modules.Link.Model
{
    public class LinkResponse : Base.Model.BaseResponseModel
    {

        public LinkResponse() {}

        public LinkResponse(Link link)
        {
            Id = link.Id;
            OriginalLink = link.OriginalLink;
            GeneratedLink = String.Format("{0}/{1}", Config.AppConfig.DomainName, link.GeneratedLink);
            TotalVisits = link.TotalVisits;
            Description = link.Description;

            CreatedAt = link.CreatedAt;
            CreatedBy = link.CreatedBy;
            UpdatedAt = link.UpdatedAt;
            DeletedAt = link.DeletedAt;
            IsDeleted = link.IsDeleted;
        }

        [JsonPropertyName("originalLink")]
        public string OriginalLink { get; set; }

        [JsonPropertyName("generatedLink")]
        public string GeneratedLink { get; set; }

        [JsonPropertyName("totalVisits")]
        public long TotalVisits { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("qrBase64")]
        public string QrBase64 { get; set; }
    }
}