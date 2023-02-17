using System.Collections.Generic;

namespace pocketbase.net.Models
{
    /// <summary>
    /// Represents tthe 
    /// </summary>
    public class RealtimeEventArgs
    {
        public string Id { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;
        public Dictionary<string, object> Data { get; set; } = new();
    }

    /// <summary>
    /// Stuct contains Actions done on topic
    /// </summary>
    public struct RealtimeActions
    {
        public const string Create = "create";
        public const string Update = "update";
        public const string Delete = "delete";

    }
}