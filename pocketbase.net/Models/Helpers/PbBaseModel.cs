using System;
using System.Collections.Generic;
using System.Text;

namespace pocketbase.net.Models.Helpers
{
    public class PbBaseModel : BaseModel
    {
        public string CollectionId { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;
    }
}
