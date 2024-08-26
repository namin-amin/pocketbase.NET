using System.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace pocketbase.net.Helpers;

public class UrlBuilder
{
    private string collectionType { get; set; } = string.Empty;
    private string recordType { get; set; } = string.Empty;
    public UrlBuilder(string collectionName)
    {
        this.collectionName = collectionName;
        if (collectionName != "admins")
        {

            collectionType = "collections/";
            recordType = "/records/";
        }
        if (collectionName == string.Empty)
        {
            this.recordType = "";
        };
    }
    private string collectionName { get; set; }

    /// <summary>
    /// builds the request url for records
    /// </summary>
    /// <param name="id">collection/record id to which request be made</param>
    /// <param name="queryParams"></param>
    /// <param name="overideColName"></param>
    /// <returns></returns>
    public string CollectionUrl(string id = "", IDictionary<string, string>? queryParams = null, string overideColName = "")
    {

        var baseUrl = "api/" +
            (overideColName == "" ? collectionType : "collections/") +
            (overideColName == "" ? collectionName.ToLower() : overideColName.ToLower()) +
            recordType + id;
        return UrlBuilderHelper.QueryBuilder(queryParams, baseUrl);
    }
    // public string CollectionUrl(string id = "")
    // {
    //     return collectionType + collectionName.ToLower() + recordType + id;
    // }

}

public static class UrlBuilderHelper
{
    /// <summary>
    /// Build query on the url provided
    /// </summary>
    /// <param name="queryParams">query details as string dcitionary</param>
    /// <param name="baseUrl">url to which query to be appended</param>
    /// <returns></returns>
    public static string QueryBuilder(IDictionary<string, string>? queryParams, string baseUrl)
    {
        if (queryParams is null) return baseUrl;

        var queryDict = queryParams
            ?.Where(c => c.Value?.ToString() != string.Empty)
            .ToDictionary(s => s.Key.ToString(), s => s.Value.ToString()) 
                        ?? new Dictionary<string, string>();

        return QueryHelpers.AddQueryString(baseUrl.EndsWith("/") == false ? baseUrl : baseUrl[..^1], queryDict);
    }
}