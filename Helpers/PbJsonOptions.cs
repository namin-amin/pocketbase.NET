using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace pocketbase.net.Helpers
{
    /// <summary>
    /// Json options used in Pocketbase communicated json
    /// </summary>
    public static class PbJsonOptions
    {
        private static JsonSerializerOptions Options = new()
        {
            Converters =
            {
               new JsonDateTimeConverter()
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static JsonSerializerOptions options { get => Options; private set => Options = value; }
    }
}
