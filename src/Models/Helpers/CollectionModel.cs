namespace pocketbase.net.Models.Helpers
{
    public class CollectionModel : BaseModel
    {
        public string name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public List<SchemaField> schema { get; set; } = new();
        public bool system { get; set; }
        public bool unique { get; set; }
        public string? listRule { get; set; }
        public string? viewRule { get; set; }
        public string? createRule { get; set; }
        public string? updateRule { get; set; }
        public string? deleteRule { get; set; }
        public Dictionary<string, dynamic> options { get; set; } = new();

    }
}