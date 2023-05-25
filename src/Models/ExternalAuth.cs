using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Models;

/// <summary>
/// External auth model
/// </summary>
public class ExternalAuth : BaseModel
{
    public string recordId { get; set; } = string.Empty;
    public string collectionName { get; set; } = string.Empty;
    public string provider { get; set; } = string.Empty;
    public string providerId { get; set; } = string.Empty;
}
