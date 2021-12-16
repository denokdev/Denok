using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Denok.Web.Modules.Link.Model
{

    public class GenerateRequest
    {
        [Required(ErrorMessage = "original link is required")]
        [JsonPropertyName("originalLink")]
        public string OriginalLink { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
    }
}