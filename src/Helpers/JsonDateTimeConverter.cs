using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;

namespace pocketbase.net.Helpers;

/// <summary>
/// Json converter option used for DateTime support
/// </summary>
public class JsonDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(DateTime));
        return DateTime.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(CultureInfo.CurrentCulture));
    }
}

