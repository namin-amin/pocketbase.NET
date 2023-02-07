using pocketbase.net.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace pocketbase.net.Models
{
    public class Admin:BaseModel
    {
        public int Avatar { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
    }
}
