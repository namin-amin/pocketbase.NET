using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace pocketbase.net.Models
{
    public class SubscriptionEventArgs
    {
        public string Id { get; set; } = string.Empty;
        public string Event { get; set; } = string.Empty;

        public SubscriptionData? Data { get; set; }
    }


    public class SubscriptionData
    {
        public string Actions { get; set; } = string.Empty;
        public Dictionary<string, object> Record { get; set; } = new();
    }

    public struct SubscriptionActions
    {
        public const string Create = "create";
        public const string Update = "update";
        public const string Delete = "delete";

    }
}