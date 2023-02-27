using System.Text.Json.Serialization;

namespace pocketbase.net.Models.Helpers
{
    public class AdminAuthModel : BaseModel
    {
        public string emailVisibility { get; set; } = string.Empty;
        public int avatar { get; set; }
    }
}