using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Models;

/// <summary>
/// Class Represents the admin auth model
/// </summary>
public class Admin : BaseModel
{
    public string avatar { get; set; } = "0";
    public string email { get; set; } = string.Empty;
}
