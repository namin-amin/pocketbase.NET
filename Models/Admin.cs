using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Models
{
    public class Admin : BaseModel
    {
        public int avatar { get; set; } = 0;
        public string email { get; set; } = string.Empty;
    }
}
