using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Denok.Web.Base.Filter
{
    public class BaseFilter
    {
        [FromQuery]
        [System.ComponentModel.DefaultValueAttribute(10)]
        [JsonPropertyName("limit")]
        public uint Limit { get; set; }

        [FromQuery]
        [JsonPropertyName("page")]
        public uint Page { get; set; }

        [JsonPropertyName("offset")]
        public uint Offset { get; protected set; }

        [FromQuery]
        [JsonPropertyName("orderBy")]
        public string OrderBy { get; set; }

        [FromQuery]
        [JsonPropertyName("sort")]
        public string Sort { get; set; }

        public void CalculateOffset()
        {
            Offset = (Page - 1) * Limit;
        }
    }
}