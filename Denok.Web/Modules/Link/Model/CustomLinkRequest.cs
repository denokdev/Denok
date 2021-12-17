using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Denok.Web.Modules.Link.Model
{

    public class CustomLinkRequest
    {
        [Required(ErrorMessage = "original link is required")]
        [JsonPropertyName("originalLink")]
        public string OriginalLink { get; set; }

        [Required(ErrorMessage = "custom link is required")]
        [JsonPropertyName("customLink")]
        public string CustomLink { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
    }
}