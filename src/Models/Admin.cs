using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Models
{
    public class Admin : BaseModel
    {
        public string avatar { get; set; } = "0";
        public string email { get; set; } = string.Empty;
    }
}
