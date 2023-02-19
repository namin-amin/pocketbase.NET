using System;
using System.Collections.Generic;
using System.Text;
using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Models
{
    public class ExternalAuth : BaseModel
    {
        public string recordId { get; set; } = string.Empty;
        public string collectionName { get; set; } = string.Empty;
        public string provider { get; set; } = string.Empty;

        public string providerId { get; set; } = string.Empty;
    }
}
