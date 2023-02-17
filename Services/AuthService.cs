using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Services
{
    public class BaseAuthService : BaseService
    {
        public event EventHandler? Onchange;
        private string _token = "";

        public BaseAuthService(HttpClient httpClient, string collectionName) : base(httpClient, collectionName)
        {
        }

        public bool IsValid
        {
            get; private set;
        }

        public string Token
        {
            get { return _token; }
        }


        public void Clear(Action<object, EventArgs>? callback = null)
        {
            _token = "";
            IsValid = false;
            //?How to format the eventargs?
            callback?.Invoke(this, new EventArgs());
            Onchange?.Invoke(this, new EventArgs());
        }

        public async Task<AdminAuthModel> AuthWithPassword(
            string email,
            string password
        )
        {

            var response = await _httpClient.PostAsJsonAsync(collectionName + "/auth-with-password", new
            {
                identity = email,
                password
            });

            var data = await
                        response.Content.ReadFromJsonAsync<IDictionary<string, object>>()
                        ??
                        new Dictionary<string, object>();


            if (data.TryGetValue("token", out object? value))
            {
                _token = value?.ToString()!;
                if (data.TryGetValue("admin", out object? thing))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<AdminAuthModel>(thing.ToString() ?? "", PbJsonOptions.Options)!;
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
                return await data.Content.ReadFromJsonAsync<AdminAuthModel>(PbJsonOptions.Options) ?? new();
            }
            return new();

        }


    }
}
