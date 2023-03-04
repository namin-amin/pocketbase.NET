namespace pocketbase.net.Models.Helpers
{
    public class AdminAuthModel : BaseModel
    {
        public bool emailVisibility { get; set; } 
        public string avatar { get; set; } = "0";
    }
}