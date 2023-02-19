using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pocketbase.net.Models.Helpers
{
    public abstract class BaseModel
    {
        public string id { get; set; } = string.Empty;
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
    }
}
