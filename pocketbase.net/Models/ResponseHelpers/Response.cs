using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace pocketbase.net.Models.ResponseHelpers
{
    public class Response<T>
        where T: class
    {
        [JsonPropertyName("code")]
        public int  Code{ get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
