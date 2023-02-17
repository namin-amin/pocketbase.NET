using System.Collections.Generic;

namespace pocketbase.net.Models.Helpers
{
    public class Record<T>
        where T : PbBaseModel
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }

    public class Record
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<IDictionary<string, object>> Items { get; set; } = new List<Dictionary<string, object>>();
    }

}

