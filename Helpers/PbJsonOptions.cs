using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace pocketbase.net.Helpers
{
    public static class PbJsonOptions
    {
        public static JsonSerializerOptions options = new JsonSerializerOptions() {
            Converters =
            {
               new JsonDateTimeConverter()
            },
            PropertyNamingPolicy= JsonNamingPolicy.CamelCase,
        };
    }
}
