﻿namespace pocketbase.net.Models.Helpers
{
    public class SchemaField
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public bool system { get; set; }
        public bool required { get; set; }
        public bool unique { get; set; }
        public IDictionary<string, dynamic> options { get; set; } = new Dictionary<string, dynamic>();

    }
}