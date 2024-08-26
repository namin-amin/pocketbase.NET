using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services.Helpers;

namespace pocketbase.net.Services;

/// <summary>
/// Base Service class defines all the basic operations that a Service class should be doing
/// </summary>
public class BaseService
{
    internal readonly Pocketbase Client;
    internal readonly HttpClient HttpClient;
    public readonly string CollectionName;

    public readonly UrlBuilder UrlBuilder;


    public BaseService(HttpClient httpClient,
                       string collectionName, Pocketbase client)
    {
        this.CollectionName = collectionName;
        HttpClient = httpClient;
        UrlBuilder = new UrlBuilder(collectionName);
        this.Client = client;
    }

    private async Task<string> GetResponse(string id = "", RequestParams? requestParams = null)
    {
        StringContent? content = null;
        requestParams ??= new RequestParams();

        if (requestParams.body != null)
        {
            content = new StringContent(Serialize(requestParams.body, PbJsonOptions.options));
        }

        var httpResult = await Client.SendAsync(UrlBuilder.CollectionUrl(id, requestParams.queryParams), HttpMethod.Get, content);
        return await httpResult.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Gets List of Record response from PocketBase
    /// </summary>
    /// <param name="batchSize">number of items to be fetched </param>
    /// <param name="queryParams">filtering query params</param>
    /// <returns>Response schema based on the type of response</returns>
    public async Task<string> GetFullList(int batchSize = 100, RecordListQueryParams? queryParams = null)
    {
        queryParams ??= new();
        var qParams = new
        Dictionary<string, string>()
        {
            {"page",queryParams.page.ToString()},
            {"perPage",batchSize.ToString()},
            {"sort",queryParams.sort},
            {"filter",queryParams.filter},
            {"expand",queryParams.expand}
        };

        try
        {
            return await GetResponse(requestParams: new()
            {
                queryParams = qParams
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "";
            // throw;
        };

    }

    /// <summary>
    /// Gets Record response from PocketBase
    /// </summary>
    /// <typeparam name="T">collection objects Type</typeparam>
    /// <returns></returns>
    public async Task<Record<T>> GetFullList<T>(int batchSize = 100, RecordListQueryParams? queryParams = null) where T : PbBaseModel
    {
        var result = await GetFullList(batchSize, queryParams);
        if (string.IsNullOrWhiteSpace(result))
        {
            return new();
        }
        return Deserialize<Record<T>>(result, PbJsonOptions.options) ?? new Record<T>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="perPage"></param>
    /// <param name="queryParams"></param>
    /// <returns></returns>
    public async Task<string> GetList(int page,
        int perPage,
       ListQueryParams? queryParams = null)
    {
        queryParams ??= new();
        var qParams = new
         Dictionary<string, string>()
        {
            {"page",page.ToString()},
            {"perPage",perPage.ToString()},
            {"sort",queryParams.sort},
            {"filter",queryParams.filter}
        };
        try
        {
            return await GetResponse(requestParams: new()
            {
                queryParams = qParams,
            });
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            return "";
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"></param>
    /// <param name="perPage"></param>
    /// <param name="queryParams"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<Record<T>?> GetList<T>(int page,
       int perPage,
    ListQueryParams? queryParams = null) where T : PbBaseModel
    {

        try
        {
            return Deserialize<Record<T>>(await GetList(page, perPage, queryParams)) ?? null;
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            throw;
        }
    }


    public async Task<T?> GetFirstListItem<T>(string filter, RecordListQueryParams? queryParams = null)
    where T : PbBaseModel
    {
        var result = await GetFirstListItem(filter, queryParams);
        return Deserialize<Record<T>>(result, PbJsonOptions.options)?.items.FirstOrDefault();
    }

    public async Task<string> GetFirstListItem(string filter, RecordListQueryParams? queryParams = null)
    {
        queryParams ??= new();
        queryParams.filter = filter;
        var qParams = new
         Dictionary<string, string>()
        {
            {"page",queryParams.page.ToString()},
            {"perPage",queryParams.perPage.ToString()},
            {"sort",queryParams.sort},
            {"filter",queryParams.filter},
            {"expand",queryParams.expand}
        };

        return await GetResponse(requestParams: new() { queryParams = qParams });
    }



    /// <summary>
    /// Gets Record response from PoccketBase
    /// </summary>
    /// <param name="id">Records id which to be got</param>
    /// <returns></returns>
    public async Task<IDictionary<string, object>> GetOne(string id, string expand = "")
    {
        var result = await GetResponse(id, new()
        {
            queryParams = new Dictionary<string, string>()
        {
            {"expand",expand.ToString()}
        }
        });
        return Deserialize<IDictionary<string, object>>(result, PbJsonOptions.options)!;
    }

    /// <summary>
    /// Gets Record response from PoccketBase
    /// </summary>
    /// <param name="id">Records id which to be got</param>
    /// <typeparam name="T">collection objects Type</typeparam>
    /// <returns></returns>
    public async Task<T> GetOne<T>(string id, string expand = "")
        where T : class, new()
    {
        var result = await GetResponse(id, new()
        {
            queryParams = new Dictionary<string, string>()
        {
            {"expand",expand.ToString()}
        }
        });
        return Deserialize<T>(result, PbJsonOptions.options) ?? new();
    }

    /// <summary>
    /// Create new Record in the collection
    /// </summary>
    /// <param name="data">Data to be inserted</param>
    /// <returns></returns>
    public async Task<IDictionary<string, object>> Create(IDictionary<string, object> data)
    {
        var response = await GetResponse(requestParams: new()
        {
            body = data,
            method = HttpMethod.Post,
        });
        return Deserialize<IDictionary<string, object>>(response) ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Create new Record in the collection
    /// </summary>
    /// <param name="data">Data to be inserted</param>
    /// <typeparam name="T">Return type expected</typeparam>
    /// <typeparam name="D">Type of input object</typeparam>
    /// <returns></returns>
    public async Task<T> Create<T, D>(D data)
        where T : class, new()
    {
        var response = await GetResponse(requestParams: new()
        {
            body = data,
            method = HttpMethod.Post,
        });
        return (Deserialize<T>(response, PbJsonOptions.options)) ?? new();
    }

    /// <summary>
    /// Update the given Record 
    /// </summary>
    /// <param name="data">data to be changed</param>
    /// <param name="id">id of the record which to be changed</param>
    /// <typeparam name="T">type of Return object</typeparam>
    /// <typeparam name="D">type of input object</typeparam>
    /// <returns></returns>
    public async Task<T> Update<T, D>(D data, string id)
        where T : class, new()
    {
        var response = await GetResponse(id, requestParams: new()
        {
            body = data,
            method = HttpMethod.Patch,
        }
        );
        return Deserialize<T>(response) ?? new();
    }
    /// <summary>
    /// Delete Record
    /// </summary>
    /// <param name="id">id of the record to be deleted</param>
    /// <returns></returns>
    public async Task<bool> Delete(string id)
    {
        var result = await HttpClient.DeleteAsync(UrlBuilder.CollectionUrl(id));
        return result.StatusCode == HttpStatusCode.OK;
    }
}