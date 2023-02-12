using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace pocketbase.net.Helpers
{
    public static class PbJsonOptions
    {
        private static JsonSerializerOptions options = new()
        {
            Converters =
            {
               new JsonDateTimeConverter()
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static JsonSerializerOptions Options { get => options; private set => options = value; }
    }
}
