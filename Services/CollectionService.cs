using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models;
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
            var result = await GetFullList();
            return JsonSerializer.Deserialize<ListModel<T>>(result, PbJsonOptions.Options) ?? new ListModel<T>();
        }

        public async Task<IDictionary<string, object>> GetOne(string id)
        {
            var result = await GetFullList(id);
            return JsonSerializer.Deserialize<IDictionary<string, object>>(result, PbJsonOptions.Options)!;
        }


        public async Task<T> GetOne<T>(string id)
        {
            var result = await GetFullList(id);
            return JsonSerializer.Deserialize<T>(result, PbJsonOptions.Options)!;
        }

        public void Subscribe(Action<SubscriptionEventArgs> callbackFun)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribeAll()
        {
            throw new NotImplementedException();
        }
    }

}