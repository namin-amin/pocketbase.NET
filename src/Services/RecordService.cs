using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using pocketbase.net.Helpers;
using pocketbase.net.Models;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services.Interfaces;

namespace pocketbase.net.Services;

public class RecordService : BaseService
{
    internal IRealtimeServiceBase realtimeService { get; }
    internal BaseAuthService<RecordAuthModel> baseAuthService { get; }

    public string baseCollectionPath
    {
        get
        {
            return $"api/collections/{Uri.EscapeDataString(collectionName)}";
        }
    }

    internal RecordService(
       HttpClient _httpClient,
       string collectionName,
       IRealtimeServiceBase realtimeService,
       Pocketbase cleint
   ) : base(_httpClient, collectionName, cleint)
    {
        this.realtimeService = realtimeService;
        baseAuthService = new(_httpClient, collectionName, cleint);
    }



    /// <summary>
    /// subscribe to the realtime events of the server
    /// </summary>
    /// <param name="topic">topic representing colection</param>
    /// <param name="callbackFun">Action to be called when there is alterations in the given topic collection</param>
    public void Subscribe(string topic, Action<RealtimeEventArgs> callbackFun)
    {
        realtimeService.Subscribe(topic, callbackFun, collectionName);
    }

    /// <summary>
    /// Unsubscribe from the current collection
    /// </summary>
    public void UnSubscribe(string topic)
    {
        realtimeService.UnSubscribe(topic);
    }

    public async Task<string> ListAuthMethods()
    {
        try
        {
            return await _httpClient.GetStringAsync($"api/collections/{collectionName}/auth-methods");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return "";
        }
    }

    public async Task<RecordAuthModel?> AuthWithPassword(
       string email,
       string password
   )
    {
        try
        {
            return await baseAuthService.AuthWithPassword(email, password, collectionName);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            return null;
        }
    }

    /// <summary>
    /// Refresh currently authenticated users Auth details
    /// </summary>
    /// <returns></returns>
    public async Task<RecordAuthModel> AuthRefresh()
    {
        return await baseAuthService.AuthRefresh();
    }

    /// <summary>
    /// Authenticate user with OAuth2
    /// </summary>
    /// <returns>
    /// <list type="bullet">
    /// <item> Auth token</item>
    /// <item>Auth record model</item>
    /// <item>auth Outh2  account data</item>
    /// </list>
    /// </returns>
    public async Task<string> AuthWithOAuth2(
        string provider,
        string code,
        string codeVerifier,
        string redirectUrl,
        Dictionary<string, dynamic>? createdData = null,
        Dictionary<string, dynamic>? body = null,
        Dictionary<string, string>? query = null,
        Dictionary<string, string>? headers = null,
        string expand = ""
    )
    {
        body ??= new();
        body.Add("provider", provider);
        body.Add("code", code);
        body.Add("codeVerifier", codeVerifier);
        body.Add("redirectUrl", redirectUrl);
        body.Add("createData", createdData ?? new());

        query ??= new();
        query.Add("expand", expand);

        string uri = $"{_httpClient.BaseAddress}{baseCollectionPath}/auth-with-oauth2";

        uri = UrlBuilderHelper.QueryBuilder(query, uri);

        var response = await cleint.SendAsync(uri, HttpMethod.Post, new StringContent(Serialize(body, PbJsonOptions.options)), headers);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return "";
    }

    public async Task<bool> RequestVerification(string email)
    {
        string uri = $"{baseCollectionPath}/request-verification";
        var content = new Dictionary<string, string>
        {
            { "email", email }
        };

        var strcontent = Serialize(content, PbJsonOptions.options);

        Console.WriteLine(strcontent);

        var response = await cleint.SendAsync(
            uri,
            HttpMethod.Post,
            new StringContent(strcontent)
        );
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return true;
        }
        return false;
    }
}