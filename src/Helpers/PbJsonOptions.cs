namespace pocketbase.net.Helpers;

/// <summary>
/// Json options used in Pocketbase communicated json
/// </summary>
public static class PbJsonOptions
{
    public static JsonSerializerOptions options { get; private set; } = new()
    {
        Converters =
        {
            new JsonDateTimeConverter()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
