using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services;
using pocketbase.net.Services.Helpers;
using pocketbase.net.Services.Interfaces;
using pocketbase.net.Store;

namespace pocketbase.net;

/// <summary>
/// Represents the Client 
/// </summary>
public class Pocketbase
{
    private HttpClient httpClient { get; }

    private readonly Dictionary<string, RecordService> _recordCollection = new();

    public AdminService admins { get; set; }

    public BaseAuthStore authStore { get; set; }
    private IRealtimeServiceBase realtimeService { get; set; }

    public CollectionService collection { get; set; }

    public string baseurl { get; set; }
    public string lang { get; set; }
    /// <summary>
    /// Init Pocketbase Client
    /// </summary>
    /// <param name="baseurl">represents the baseurl of the Pocketbase server</param>
    /// <param name="lang">Language Preference</param>
    /// <param name="httpClient">Provide a HttpClient to be used if already initialized else pass null New one will be created</param>
    public Pocketbase(string baseurl,
                      string? lang,
                      HttpClient? httpClient)
    {
        this.httpClient = httpClient ?? new HttpClient();
        this.baseurl = baseurl.EndsWith("/") ? baseurl : baseurl + "/";
        this.httpClient.BaseAddress = new Uri(baseurl);
        this.lang = lang ?? "en-US";
        realtimeService = new RealtimeService(this.httpClient, baseurl);
        authStore = new();
        admins = new(this.httpClient, this);
        collection = new(this.httpClient, this);
    }

    /// <summary>
    /// Init Pocketbase Cleint
    /// </summary>
    /// <param name="baseurl">represents the baseurl of the Pocketbase server</param>
    /// <param name="lang">Language Preference</param>
    /// <param name="httpClient">Provide a HttpCleint to be used if already initialiased else pass null New one will be created</param>
    /// <param name="realtimeService"></param>
    public Pocketbase(string baseurl,
                      string lang = "en-US",
                      HttpClient? httpClient = null,
                      IRealtimeServiceBase? realtimeService = null)
    {
        this.httpClient = httpClient ?? new HttpClient();
        this.baseurl = baseurl.EndsWith("/") ? baseurl : baseurl + "/";
        this.httpClient.BaseAddress = new Uri(baseurl);
        this.lang = lang ?? "en-US";
        this.realtimeService = realtimeService ?? new RealtimeService(this.httpClient, baseurl);
        authStore = new();
        admins = new(this.httpClient, this);
        collection = new(this.httpClient, this);
    }

    /// <summary>
    /// Init Pocketbase Cleint
    /// </summary>
    /// <param name="baseurl">represents the baseurl of the Pocketbase server</param>
    /// <param name="httpClient">Provide a HttpClient to be used if already initialized else pass null New one will be created</param>
    public Pocketbase(string baseurl,
                     HttpClient? httpClient)
    {
        this.httpClient = httpClient ?? new HttpClient();
        this.baseurl = baseurl.EndsWith("/") ? baseurl : baseurl + "/";
        this.httpClient.BaseAddress = new Uri(baseurl);
        this.lang = "en-US";
        realtimeService = new RealtimeService(this.httpClient, baseurl);
        authStore = new();
        admins = new(this.httpClient, this);
        collection = new(this.httpClient, this);
    }

    // public BaseAuthStore AuthStore { get; set; }

    /// <summary>
    /// Returns the Collection object that can be used to do CRUD and Subscriptions
    /// </summary>
    /// <param name="collectionName">Name of the collection to get</param>
    /// <returns></returns>
    public RecordService Collections(string collectionName)
    {
        RecordService? collectionService;
        if (_recordCollection.TryGetValue(collectionName, out var value))
        {
            collectionService = value;
            return collectionService;
        }

        collectionService = new(httpClient, collectionName, realtimeService, this);
        _recordCollection.Add(collectionName, collectionService);
        return collectionService;
    }


    /// <summary>
    /// Send requests to pb server
    /// </summary>
    /// <param name="url">url to connect and send with details of operation</param>
    /// <param name="httpMethod">http method to be used</param>
    /// <param name="content">Content to be sent</param>
    /// <param name="headers"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> SendAsync(string url, 
                                                        HttpMethod httpMethod, 
                                                        StringContent? content = null, 
                                                        IDictionary<string, string>? headers = null)
    {
        try
        {
            var message = new HttpRequestMessage
            {
                RequestUri = new Uri(baseurl!.ToString() + url),
                Method = httpMethod,
            };

            if (content != null && httpMethod != HttpMethod.Get) message.Content = content;
            if (authStore.token != "")
                message.Headers.Authorization = new AuthenticationHeaderValue(authStore.token);

            if (headers is null)
            {
                return await httpClient.SendAsync(message);
            }

            foreach (var header in headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            return await httpClient.SendAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new(HttpStatusCode.InternalServerError);
        }

    }

    public string GetFileUrl(PbBaseModel model, string fileName, FileQueryParams? fileQueryParams = null)
    {
        fileQueryParams ??= new();
        List<string> files = new()
        {
            "api",
            "files",
            model.collectionId != "" ? model.collectionId : model.collectionName,
            model.id,
            fileName
        };

        var url = baseurl + string.Join("/", files);
        url = url.EndsWith("/") ? url[..^1] : url;

        url = UrlBuilderHelper.QueryBuilder(new Dictionary<string, string>()
        {
            { "thumb" , fileQueryParams.thumb }
        }, url);

        return url;
    }

}

public static class PocketBaseProvider
{
    /// <summary>
    /// Extension method adds a Pocketbase To the Dependency injection services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="baseUrl">represents the baseurl of the Pocketbase server</param>
    /// <param name="httpClient">Provide a HttpClient to be used if already initialized else pass null New one will be created</param>
    /// <param name="lang">Language Preference</param>
    /// <returns></returns>
    public static IServiceCollection AddPocketbase(this IServiceCollection services, string baseUrl, HttpClient? httpClient, string? lang = "en-US")
    {

        //return services.AddSingleton<Pocketbase>(new Pocketbase(baseUrl,lang));

        return services.AddSingleton((options) => new Pocketbase(baseUrl, lang, httpClient));
    }


    /// <summary>
    /// Extension method adds a Pocketbase To the Dependency injection services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="baseUrl">represents the baseurl of the Pocketbase server</param>
    /// <param name="lang">Language Preference</param>
    public static IServiceCollection AddPocketbase(this IServiceCollection services, string baseUrl, string? lang = "en-US")
    {

        return services.AddSingleton((options) => new Pocketbase(baseUrl, lang, new HttpClient()));
    }
}
