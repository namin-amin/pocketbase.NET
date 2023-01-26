using pocketbase.net.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace pocketbase.net.Models
{
    public class Collection :BaseModel
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public IEnumerable<SchemaField> Schema { get; set; } = new List<SchemaField>();
        public bool System{ get; set; }
        public string? ListRule { get; set; }
        public string? ViewRule  { get; set; }
        public string? CreateRule { get; set; }
        public string? UpdateRule { get; set; }
        public string? DeleteRule { get; set;}
        public IDictionary<string,object> Options{ get; set; } = new Dictionary<string,object>();
        public bool IsBase 
        {
            get { return Type == "base"; }
        }
        public bool IsAuth 
        { 
            get { return Type == "auth"; } 
        }
        public bool IsSingle 
        {
            get { return Type == "single"; } 
        }


    }
}
