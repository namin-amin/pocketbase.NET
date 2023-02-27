using System.Collections.Generic;

namespace pocketbase.net.Models.Helpers
{
    public class Record<T> : BaseRecord
        where T : PbBaseModel
    {
        public int totalPages { get; set; }

        public IEnumerable<T> items { get; set; } = new List<T>();
    }

    public class ColRecord<T> : BaseRecord
        where T : BaseModel
    {
        public int totalPages { get; set; }

        public IEnumerable<T> items { get; set; } = new List<T>();
    }

    public class Record : BaseRecord
    {
        public int totalPages { get; set; }
        public IEnumerable<IDictionary<string, object>> items { get; set; } = new List<Dictionary<string, object>>();
    }

    public class AdminRecord : BaseRecord
    {
        public IEnumerable<AdminAuthModel> items { get; set; } = new List<AdminAuthModel>();
    }
    public abstract class BaseRecord
    {
        public int page { get; set; }
        public int perPage { get; set; }
        public int totalItems { get; set; }
    }
}

