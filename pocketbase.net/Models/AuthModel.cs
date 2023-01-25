using System.Text.Json.Serialization;

namespace pocketbase.net.Models
{
    public class AuthModel : BaseModel
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("emailVisibility")]
        public string EmailVisibility { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("avatar")]
        public string FileName { get; set; } = string.Empty;
    }
}