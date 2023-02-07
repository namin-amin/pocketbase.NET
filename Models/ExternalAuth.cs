using pocketbase.net.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace pocketbase.net.Models
{
    public class ExternalAuth:BaseModel
    {
        public string RecordId { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        
        public string ProviderId { get; set; } = string.Empty;
    }
}
