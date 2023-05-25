namespace pocketbase.net.Models.Helpers;

/// <summary>
/// Model represents pocketbase collection base class
/// </summary>
public class PbBaseModel : BaseModel
{
    public string collectionId { get; set; } = string.Empty;
    public string collectionName { get; set; } = string.Empty;

    public Dictionary<string, dynamic> expand { get; set; } = new();

}
