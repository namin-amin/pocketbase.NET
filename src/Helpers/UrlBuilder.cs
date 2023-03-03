using System.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace pocketbase.net.Helpers
{
    public class UrlBuilder
    {

        public string collectionType { get; set; } = string.Empty;
        public string recordType { get; set; } = string.Empty;
        public UrlBuilder(string collectionName)
        {

            if (collectionName != "admins")
            {
                this.collectionName = collectionName;
                collectionType = "collections/";
                recordType = "/records/";
            }
            if (collectionName == string.Empty)
            {
                this.recordType = "";
            };
        }
        private string collectionName { get; set; } = string.Empty;

        /// <summary>
        /// builds the request url for records
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string CollectionUrl(string id = "", IDictionary<string, string>? queryParams = null)
        {
            var baseUrl = "api/" + collectionType + collectionName.ToLower() + recordType + id;
            return QueryBuilder(queryParams, baseUrl);
        }
        // public string CollectionUrl(string id = "")
        // {
        //     return collectionType + collectionName.ToLower() + recordType + id;
        // }

        private static string QueryBuilder(IDictionary<string, string>? queryParams, string baseUrl)
        {
            if (queryParams is null) return baseUrl;

            var queryDict = queryParams
                ?.Where(
                    c =>
                    {
                        {
                            if (c.Value != null)
                                if (c.Value?.ToString() != string.Empty)
                                {
                                    return true;
                                }
                        }
                        return false;
                    }
                ).ToDictionary(s => s.Key.ToString(), s => s.Value.ToString()) ?? new();

            return QueryHelpers.AddQueryString(baseUrl[^2..] == "/" ? baseUrl : baseUrl[..^1], queryDict);

        }



    }
}