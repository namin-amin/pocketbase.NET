using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pocketbase.net.Services.Helpers
{
    public enum ListQueryParams
    {
        page,
        perPage,
        sort,
        filter
    }

    //check if required can use starightup things
    public class RecordQueryParams
    {
        public string expand { get; set; } = string.Empty;
    }

    public enum RecordListQueryParams
    {
        page,
        perPage,
        sort,
        filter,
        expand
    }

    public class LogStatsQueryParams
    {
        public string filter { get; set; } = string.Empty;
    }

    public class FileQueryParams
    {
        public string thumb { get; set; } = string.Empty;
    }

    public enum Filey
    {
        Filter,
        Sort,
        Thumb
    }
}