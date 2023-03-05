namespace pocketbase.net.Models.Helpers;

/// <summary>
/// Authmodel representing user auth model
/// </summary>
public class RecordAuthModel : PbBaseModel
{
    public bool emailVisibility { get; set; }
    public string avatar { get; set; } = "";
    public bool verified { get; set; }
    public string username { get; set; } = "";
    public string email { get; set; } = "";
    public string name { get; set; } = "";

}