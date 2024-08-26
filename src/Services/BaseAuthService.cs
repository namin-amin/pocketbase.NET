using System.Net.Http;
using System.Net.Http.Json;
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
        url = url == "" ? UrlBuilder.CollectionUrl() : UrlBuilder.CollectionUrl(overideColName: url);
        url += "/auth-with-password";

        var response = await HttpClient.PostAsJsonAsync(
          url, new
          {
              identity = email,
              password
          });

        var data = await
                    response.Content.ReadFromJsonAsync<IDictionary<string, object>>()
                    ??
                    new Dictionary<string, object>();

        //object thing = null;

        if (data.TryGetValue("token", out object? value))
        {
            Client.authStore.token = value?.ToString()!;
            if (data.TryGetValue("admin", out object? admin))
            {
                try
                {
                    Client.authStore.model = Deserialize<T>(admin?.ToString() ?? "", PbJsonOptions.options) ?? new();
                    return Client.authStore.model;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            if (data.TryGetValue("record", out object? record))
            {
                try
                {
                    Client.authStore.model = Deserialize<T>(record?.ToString() ?? "", PbJsonOptions.options) ?? new();
                    return Client.authStore.model;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            return new();
        }
        return new();
    }

    /// <summary>
    /// Refresh currently authenticated users Auth details
    /// </summary>
    /// <returns></returns>
    public async Task<RecordAuthModel> AuthRefresh()
    {
        var data = await HttpClient.PostAsJsonAsync("admins", new
        {
            Authorization = Client.authStore.token
        });


        if (data.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await data.Content.ReadFromJsonAsync<RecordAuthModel>(PbJsonOptions.options) ?? new();
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

        var data = await HttpClient.PostAsJsonAsync("admins", new
        {
            email,
            password,
            passwordConfirm,
            avatar,
            Authorization = Client.authStore.token
        });


        if (data.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return await data.Content.ReadFromJsonAsync<RecordAuthModel>(PbJsonOptions.options) ?? new();
        }
        return new();

    }

    public async Task<bool> RequestPasswordReset(string email)
    {
        var data = await HttpClient.PostAsJsonAsync("admins/request-password-reset", new
        {
            email,
            Authorization = Client.authStore.token
        });

        if (data.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> ConfirmPasswordReset(string email, string password, string passwordConfirm)
    {
        var data = await HttpClient.PostAsJsonAsync("admins/confirm-password-reset", new
        {
            email,
            password,
            passwordConfirm,
            Authorization = Client.authStore.token
        });

        if (data.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return true;
        }
        return false;
    }
}
