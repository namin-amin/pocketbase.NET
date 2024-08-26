namespace pocketbase.net.Models;

/// <summary>
/// Represents Realtime event args 
/// </summary>
public class RealtimeEventArgs : EventArgs
{
    public string id { get; set; } = string.Empty;
    public string @event { get; init; } = string.Empty;//this rule violation not fixed as it conflicts with event keyword
    //public Dictionary<string, object> data { get; set; } = new();

    public Data data { get; init; } = new();

    public override string ToString()
    {
        return Serialize(this.MemberwiseClone());
    }
}

public class Data
{
    public string action { get; init; } = "";

    public Dictionary<string, object> record { get; init; } = new();
}

/// <summary>
/// Struct contains Actions done on topic
/// </summary>
public struct RealtimeActions
{
    public const string Create = "create";
    public const string Update = "update";
    public const string Delete = "delete";

}