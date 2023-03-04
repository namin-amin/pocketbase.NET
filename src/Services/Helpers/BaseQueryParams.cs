using System.Net.Http;

namespace pocketbase.net.Services.Helpers
{
    public class ListQueryParams
    {
        public int page = 0;
        public int perPage = 0;
        public string sort = string.Empty;
        public string filter = string.Empty;
    }

    //check if required can use starightup things
    public class RecordQueryParams
    {
        public string expand { get; set; } = string.Empty;
    }

    public class RecordListQueryParams
    {
        public int page = 0;
        public int perPage = 0;
        public string sort = string.Empty;
        public string filter = string.Empty;
        public string expand = string.Empty;
    }

    public class LogStatsQueryParams
    {
        public string filter { get; set; } = string.Empty;
    }

    public class FileQueryParams
    {
        public string thumb { get; set; } = string.Empty;
    }

    public class RequestParams
    {
        public HttpMethod method { get; set; } = HttpMethod.Get;
        public Dictionary<string, string>? queryParams { get; set; } = null;
        public object? body { get; set; } = null;
    }

}