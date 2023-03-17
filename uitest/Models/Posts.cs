using pocketbase.net.Models.Helpers;

namespace uitest.Models
{
    public class Posts : PbBaseModel
    {
        public string title { get; set; } = string.Empty;
        public string details { get; set; } = string.Empty;
    }
}