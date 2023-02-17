using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Services
{
    /// <summary>
    /// Collection service class defines the services available on the colllections
    /// </summary>
    public class CollectionService : BaseService
    {

        public CollectionService(HttpClient _httpClient, string collectionName) : base(_httpClient, collectionName)
        {
        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <typeparam name="T">collection objects Type</typeparam>
        /// <returns></returns>
        public async Task<Record<T>> GetFullList<T>() where T : PbBaseModel
        {
            var result = await GetFullList();
            return JsonSerializer.Deserialize<Record<T>>(result, PbJsonOptions.Options) ?? new Record<T>();
        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="id">Records id which to be got</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> GetOne(string id)
        {
            var result = await GetFullList(id);
            return JsonSerializer.Deserialize<IDictionary<string, object>>(result, PbJsonOptions.Options)!;
        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="id">Records id which to be got</param>
        /// <typeparam name="T">collection objects Type</typeparam>
        /// <returns></returns>
        public async Task<T> GetOne<T>(string id)
        {
            var result = await GetFullList(id);
            return JsonSerializer.Deserialize<T>(result, PbJsonOptions.Options)!;
        }

    }

}