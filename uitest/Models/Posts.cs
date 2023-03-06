using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pocketbase.net.Models.Helpers;

namespace uitest.Models
{
    public class Posts : PbBaseModel
    {
        public string title { get; set; } = string.Empty;
        public string details { get; set; } = string.Empty;

        public override string ToString()
        {
            return base.ToString();
        }
    }
}