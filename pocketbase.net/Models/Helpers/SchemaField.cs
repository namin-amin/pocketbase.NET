using System;
using System.Collections.Generic;
using System.Text;

namespace pocketbase.net.Models.Helpers
{
    public class SchemaField
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool System { get; set; }
        public bool Required { get; set; }
        public bool Unique { get; set; }
        public IDictionary<string,object> Options { get; set; } = new Dictionary<string,object>();

    }
}
