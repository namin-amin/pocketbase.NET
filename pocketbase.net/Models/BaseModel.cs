using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pocketbase.net.Models
{
    public abstract class BaseModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("collectionId")]
        public string CollectionId { get; set; } = string.Empty;

        [JsonPropertyName("collectionName")]
        public string CollectionName { get; set; } = string.Empty;

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }
    }
}
