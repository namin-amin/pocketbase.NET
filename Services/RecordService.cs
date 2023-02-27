using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models;
using pocketbase.net.Models.Helpers;
using static System.Text.Json.JsonSerializer;

namespace pocketbase.net.Services
{
    public class RecordService : BaseService
    {
        internal RealtimeService realtimeService { get; }
        internal RecordService(
           HttpClient _httpClient,
           string collectionName,
           RealtimeService realtimeService,
           Pocketbase cleint
       ) : base(_httpClient, collectionName, cleint)
        {
            this.realtimeService = realtimeService;
        }



        /// <summary>
        /// subscribe to the realtime events of the server
        /// </summary>
        /// <param name="topic">topic representing colection</param>
        /// <param name="callbackFun">Action to be called when there is alterations in the given topic collection</param>
        public void Subscribe(string topic, Action<RealtimeEventArgs> callbackFun)
        {
            realtimeService.Subscribe(topic, callbackFun);
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
            return await _httpClient.GetStringAsync($"api/collections/{collectionName}/auth-methods");
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
                cleint.authStore._token = value?.ToString()!;
                if (data.TryGetValue("admin", out object? thing))
                {
                    try
                    {
                        return Deserialize<AdminAuthModel>(thing.ToString() ?? "", PbJsonOptions.options)!;
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
    }
}