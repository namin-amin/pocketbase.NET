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
        public event EventHandler? Onchange;
        internal string _token = "";

        public BaseAuthService(HttpClient httpClient, string collectionName, Pocketbase cleint) : base(httpClient, collectionName, cleint)
        {
        }

        public bool isValid
        {
            get; private set;
        }

        public string token
        {
            get { return _token; }
        }


        public void Clear(Action<object, EventArgs>? callback = null)
        {
            _token = "";
            isValid = false;
            //?How to format the eventargs?
            callback?.Invoke(this, new EventArgs());
            Onchange?.Invoke(this, new EventArgs());
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
                _token = value?.ToString()!;
                if (data.TryGetValue("admin", out object? admin) )
                {
                    try
                    {
                        return Deserialize<T>(admin?.ToString() ?? "", PbJsonOptions.options)!;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                }
                if (data.TryGetValue("record", out object? record))
                {
                   Console.WriteLine($"{record.ToString()}");
                    Console.WriteLine(typeof(T).Name.ToString());

                    try
                    {
                        return Deserialize<T>(record?.ToString() ?? "", PbJsonOptions.options)!;
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

        public async Task<AdminAuthModel> Refresh()
        {
            var data = await _httpClient.PostAsJsonAsync("admins", new
            {
                Authorization = _token
            });


            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await data.Content.ReadFromJsonAsync<AdminAuthModel>(PbJsonOptions.options) ?? new();
            }
            return new();
        }

        public async Task<AdminRecord> GetFullList()
        {
            return Deserialize<AdminRecord>(await base.GetFullList()) ?? new();
        }

        public new async Task<AdminAuthModel> GetOne(string id, string expand)
        {
            return await GetOne<AdminAuthModel>(id, expand);
        }


        public async Task<AdminAuthModel> Update(dynamic data, string id)
        {
            return await Update<AdminAuthModel, dynamic>(data, id);
        }


        public async Task<AdminAuthModel?> Create(
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
                Authorization = _token
            });


            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await data.Content.ReadFromJsonAsync<AdminAuthModel>(PbJsonOptions.options) ?? new();
            }
            return new();

        }

        public async Task<bool> RequestPasswordReset(string email)
        {
            var data = await _httpClient.PostAsJsonAsync("admins/request-password-reset", new
            {
                email,
                Authorization = _token
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
                Authorization = _token
            });

            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

    }
}
