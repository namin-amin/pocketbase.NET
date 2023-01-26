using pocketbase.net.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace pocketbase.net.Models
{
    public class LogRequest:BaseModel
    {
        public string   Url { get; set; } = string.Empty;
        public string Method { get; set; } = "GET";
        public int Status { get; set; } = 200;
        public string Auth { get; set; } = "guest";
        public string RemoteIp { get; set; }  = string.Empty;
        public string UserIp { get; set; } = string.Empty;
        public string Referer { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public IDictionary<string, object> Meta { get; set; } = new Dictionary<string, object>();

    }
}
