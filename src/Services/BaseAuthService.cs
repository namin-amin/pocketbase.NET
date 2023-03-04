using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Services
{
    public class BaseAuthService<T> : BaseService
        where T : class,new()
    {
        

        public BaseAuthService(HttpClient httpClient, string collectionName, Pocketbase cleint) : base(httpClient, collectionName, cleint)
        {
        }


        public async Task<T> AuthWithPassword(
            string email,
            string password,
            string url = ""
        )
        {
            url = url == "" ? urlBuilder.CollectionUrl():urlBuilder.CollectionUrl(overideColName:url);
            url = url + "/auth-with-password";
            var response = await _httpClient.PostAsJsonAsync(
              url  , new
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
                cleint.authStore.token =  value?.ToString()!;
                if (data.TryGetValue("admin", out object? admin) )
                {
                    try
                    {
                        cleint.authStore.model = Deserialize<T>(admin?.ToString() ?? "", PbJsonOptions.options);
                        return cleint.authStore.model;
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
                        cleint.authStore.model = Deserialize<T>(record?.ToString() ?? "", PbJsonOptions.options);
                        return cleint.authStore.model;
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

        public async Task<RecordAuthModel> Refresh()
        {
            var data = await _httpClient.PostAsJsonAsync("admins", new
            {
                Authorization = cleint.authStore.token
            });


            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await data.Content.ReadFromJsonAsync<RecordAuthModel>(PbJsonOptions.options) ?? new();
            }
            return new();
        }

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

            var data = await _httpClient.PostAsJsonAsync("admins", new
            {
                email,
                password,
                passwordConfirm,
                avatar,
                Authorization = cleint.authStore.token
            });


            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await data.Content.ReadFromJsonAsync<RecordAuthModel>(PbJsonOptions.options) ?? new();
            }
            return new();

        }

        public async Task<bool> RequestPasswordReset(string email)
        {
            var data = await _httpClient.PostAsJsonAsync("admins/request-password-reset", new
            {
                email,
                Authorization = cleint.authStore.token
            });

            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ConfirmPasswordReset(string email, string password, string passwordConfirm)
        {
            var data = await _httpClient.PostAsJsonAsync("admins/confirm-password-reset", new
            {
                email,
                password,
                passwordConfirm,
                Authorization = cleint.authStore.token
            });

            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

    }
}
