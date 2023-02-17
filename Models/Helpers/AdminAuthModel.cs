using System.Text.Json.Serialization;

namespace pocketbase.net.Models.Helpers
{
    public class AdminAuthModel : BaseModel
    {
        public string EmailVisibility { get; set; } = string.Empty;
        public int Avatar { get; set; }
    }
}