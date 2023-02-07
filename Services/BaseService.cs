using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;

namespace pocketbase.net.Services
{
    /// <summary>
    /// Base Service class defines all the basic operatins that a Service class shud be doing
    /// </summary>
    public abstract class BaseService
    {
        public readonly HttpClient _httpClient;
        private readonly string collectionName;

        public BaseService(HttpClient httpClient, string collectionName)
        {
            this.collectionName = collectionName;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="collectionName">name of the collection to be queried</param>
        /// <param name="id">If getting single record then required  id</param>
        /// <returns>Response schema based on the type of response</returns>
        public async Task<string> GetFullList(string id = "")
        {
            string? requesturl = "collections/" + collectionName.ToLower() + "/records/" + id;
            var httpresult = await _httpClient!.GetAsync(requesturl);
            return await httpresult.Content.ReadAsStringAsync();
        }

        public async Task<IDictionary<string, object>> Create(IDictionary<string, object> data)
        {
            string? requesturl = "collections/" + collectionName.ToLower() + "/records/";
            var response = await _httpClient.PostAsJsonAsync(requesturl, data);
            return await response.Content.ReadAsAsync<IDictionary<string, object>>();
        }

        public async Task<T> Create<T, D>(D data)
        {
            string? requesturl = "collections/" + collectionName.ToLower() + "/records/";
            var response = await _httpClient.PostAsJsonAsync(requesturl, data);
            return await response.Content.ReadAsAsync<T>();
        }

        public async Task<T> Update<T, D>(D data, string id)
        {
            string? requesturl = "collections/" + collectionName.ToLower() + "/records/" + id;
            var response = await _httpClient.PatchAsync(requesturl,

                new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json")
            );
            return await response.Content.ReadAsAsync<T>();
        }
    }
}
