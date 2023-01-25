using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace pocketbase.net.Models
{
    public class ListModel<T> 
        where T:BaseModel
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("perPage")]
        public int PerPage { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }
        
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
