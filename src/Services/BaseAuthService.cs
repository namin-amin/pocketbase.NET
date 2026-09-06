using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Services;

/// <summary>
/// Common authetication related services
/// </summary>
/// <typeparam name="T">Type of the authentication model RecordAuthmodel or Admin</typeparam>
public class BaseAuthService<T> : BaseService
    where T : class, new()
{


    public BaseAuthService(HttpClient httpClient, string collectionName, Pocketbase client) : base(httpClient, collectionName, client)
    {
    }


    /// <summary>
    /// Authenticate with password and email
    /// </summary>
    /// <param name="email">email or username representng the authrecord / admin</param>
    /// <param name="password">password of the user</param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<T> AuthWithPassword(
        string email,
        string password,
        string url = ""
    )
    {
        var collection = url == "" ? CollectionName : url;
        var endpoint = $"api/collections/{Uri.EscapeDataString(collection)}/auth-with-password";
        var response = await Client.SendAsync(endpoint, HttpMethod.Post,
            new StringContent(Serialize(new { identity = email, password }, PbJsonOptions.options)));

        if (!response.IsSuccessStatusCode)
            return new();

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;
        if (root.TryGetProperty("token", out var token))
        {
            Client.authStore.token = token.GetString() ?? string.Empty;
            Client.authStore.MarkValid();
            if (root.TryGetProperty("admin", out var admin))
            {
                Client.authStore.model = Deserialize<T>(admin.GetRawText(), PbJsonOptions.options) ?? new();
                return Client.authStore.model;
            }
            if (root.TryGetProperty("record", out var record))
            {
                Client.authStore.model = Deserialize<T>(record.GetRawText(), PbJsonOptions.options) ?? new();
                return Client.authStore.model;
            }
        }
        return new();
    }

    /// <summary>
    /// Refresh currently authenticated users Auth details
    /// </summary>
    /// <returns></returns>
    public async Task<RecordAuthModel> AuthRefresh()
    {
        var endpoint = $"api/collections/{Uri.EscapeDataString(CollectionName)}/auth-refresh";
        var data = await Client.SendAsync(endpoint, HttpMethod.Post);


        if (data.IsSuccessStatusCode)
        {
            using var document = JsonDocument.Parse(await data.Content.ReadAsStringAsync());
            if (document.RootElement.TryGetProperty("token", out var token))
                Client.authStore.token = token.GetString() ?? string.Empty;
            if (document.RootElement.TryGetProperty("record", out var record))
            {
                var model = Deserialize<RecordAuthModel>(record.GetRawText(), PbJsonOptions.options) ?? new();
                Client.authStore.model = model;
                Client.authStore.MarkValid();
                return model;
            }
        }
        return new();
    }


    ///TODO implementing external auth and making this a abstarct class

    /// <summary>
    /// Getlist of Auth details
    /// </summary>
    /// <returns></returns>
    public async Task<AdminRecord> GetFullList()
    {
        return Deserialize<AdminRecord>(await base.GetFullList()) ?? new();
    }

    public new async Task<RecordAuthModel> GetOne(string id, string expand)
    {
        return await GetOne<RecordAuthModel>(id, expand);
    }


    public async Task<RecordAuthModel> Update(dynamic data, string id)
    {
        return await Update<RecordAuthModel, dynamic>(data, id);
    }


    public async Task<RecordAuthModel?> Create(
         string email,
         string password,
         string passwordConfirm,
         int avatar
        )
    {

        var data = await Client.SendAsync($"api/collections/{Uri.EscapeDataString(CollectionName)}", HttpMethod.Post,
            new StringContent(Serialize(new
        {
            email,
            password,
            passwordConfirm,
            avatar
        }, PbJsonOptions.options)));


        if (data.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await data.Content.ReadFromJsonAsync<RecordAuthModel>(PbJsonOptions.options) ?? new();
        }
        return new();

    }

    public async Task<bool> RequestPasswordReset(string email)
    {
        var data = await Client.SendAsync($"api/collections/{Uri.EscapeDataString(CollectionName)}/request-password-reset", HttpMethod.Post,
            new StringContent(Serialize(new
        {
            email
        }, PbJsonOptions.options)));

        if (data.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> ConfirmPasswordReset(string email, string password, string passwordConfirm)
    {
        var data = await Client.SendAsync($"api/collections/{Uri.EscapeDataString(CollectionName)}/confirm-password-reset", HttpMethod.Post,
            new StringContent(Serialize(new
        {
            email,
            password,
            passwordConfirm
        }, PbJsonOptions.options)));

        if (data.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }
}
