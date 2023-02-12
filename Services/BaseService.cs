using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;

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

        public BaseService(HttpClient httpClient,
                           string collectionName)
        {
            this.collectionName = collectionName;
            _httpClient = httpClient;
            urlBuilder = new UrlBuilder(collectionName);
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

        public async Task<IDictionary<string, object>> Create(IDictionary<string, object> data)
        {
            var response = await _httpClient.PostAsJsonAsync(urlBuilder.CollectionUrl(), data);
            return (await response.Content.ReadFromJsonAsync<IDictionary<string, object>>(PbJsonOptions.Options))!;
        }

        public async Task<T> Create<T, D>(D data)
        {
            var response = await _httpClient.PostAsJsonAsync(urlBuilder.CollectionUrl(), data);
            return (await response.Content!.ReadFromJsonAsync<T>(PbJsonOptions.Options))!;
        }

        public async Task<T> Update<T, D>(D data, string id)
        {
            var response = await _httpClient.PatchAsync(urlBuilder.CollectionUrl(id),

                new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json")
            );
            return (await response.Content.ReadFromJsonAsync<T>(PbJsonOptions.Options))!;
        }
        public async void Delete(string id)
        {
            await _httpClient.DeleteAsync(urlBuilder.CollectionUrl(id));
        }
    }
}
