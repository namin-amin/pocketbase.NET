using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading;
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

        public async Task<ListModel<T>> GetFullList<T>() where T : PbBaseModel
        {
            var result = await GetFullList(typeof(T).Name.ToLower());
            return JsonSerializer.Deserialize<ListModel<T>>(result, PbJsonOptions.options) ?? new ListModel<T>();
        }

        public async Task<IDictionary<string, object>> GetOne(string id)
        {
            var result = await GetFullList(id);
            return JsonSerializer.Deserialize<IDictionary<string, object>>(result, PbJsonOptions.options)!;
        }


        public async Task<T> GetOne<T>(string id)
        {
            var result = await GetFullList(id);
            return JsonSerializer.Deserialize<T>(result, PbJsonOptions.options)!;
        }
    }

}