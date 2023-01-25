using System.Text.Json.Serialization;

namespace pocketbase.net.Models
{
    public class AuthModel : BaseModel
    {
        public string UserName { get; set; } = string.Empty;
        public bool Verified { get; set; }
        public string EmailVisibility { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}