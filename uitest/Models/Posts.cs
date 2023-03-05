using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pocketbase.net.Models.Helpers;

namespace uitest.Models
{
    public class Posts : PbBaseModel
    {
        public string post { get; set; } = string.Empty;
        public string test { get; set; } = string.Empty;
    }
}