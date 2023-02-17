using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models;

namespace pocketbase.net.Services
{
    /// <summary>
    /// Base Service class defines all the basic operatins that a Service class shud be doing
    /// </summary>
    public abstract class BaseService
    {
        public readonly HttpClient _httpClient;
        public readonly string collectionName;

        private readonly UrlBuilder urlBuilder;
        private readonly RealtimeService realtimeService;

        public BaseService(HttpClient httpClient,
                           string collectionName)
        {
            this.collectionName = collectionName;
            _httpClient = httpClient;
            urlBuilder = new UrlBuilder(collectionName);
            realtimeService = new RealtimeService(_httpClient.BaseAddress?.ToString() ?? "", httpClient);
        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="collectionName">name of the collection to be queried</param>
        /// <param name="id">If getting single record then required  id</param>
        /// <returns>Response schema based on the type of response</returns>
        public async Task<string> GetFullList(string id = "")
        {

            var httpresult = await _httpClient!.GetAsync(urlBuilder.CollectionUrl(id));
            return await httpresult.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Create new Record in the collection
        /// </summary>
        /// <param name="data">Data to be inserted</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> Create(IDictionary<string, object> data)
        {
            var response = await _httpClient.PostAsJsonAsync(urlBuilder.CollectionUrl(), data);
            return (await response.Content.ReadFromJsonAsync<IDictionary<string, object>>(PbJsonOptions.Options))!;
        }

        /// <summary>
        /// Create new Record in the collection
        /// </summary>
        /// <param name="data">Data to be inserted</param>
        /// <typeparam name="T">Return type expected</typeparam>
        /// <typeparam name="D">Type of input object</typeparam>
        /// <returns></returns>
        public async Task<T> Create<T, D>(D data)
        {
            var response = await _httpClient.PostAsJsonAsync(urlBuilder.CollectionUrl(), data);
            return (await response.Content!.ReadFromJsonAsync<T>(PbJsonOptions.Options))!;
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
        {
            var response = await _httpClient.PatchAsync(urlBuilder.CollectionUrl(id),

                new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json")
            );
            return (await response.Content.ReadFromJsonAsync<T>(PbJsonOptions.Options))!;
        }
        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="id">id of the record to be deleted</param>
        /// <returns></returns>
        public async void Delete(string id)
        {
            await _httpClient.DeleteAsync(urlBuilder.CollectionUrl(id));
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
    }
}
