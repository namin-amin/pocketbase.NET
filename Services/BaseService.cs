using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services.Helpers;

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

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <typeparam name="T">collection objects Type</typeparam>
        /// <returns></returns>
        public async Task<Record<T>> GetFullList<T>() where T : PbBaseModel
        {
            var result = await GetFullList();
            return JsonSerializer.Deserialize<Record<T>>(result, PbJsonOptions.options) ?? new Record<T>();
        }


        public async Task<string> GetList(int Page,
            int PerPage,
            ListQueryParams? queryParams = null)
        {
            // queryParams ??= new();
            // queryParams.page = Page;
            // queryParams.perPage = PerPage;


            var url = urlBuilder.CollectionUrl(queryParams: queryParams);

            var tt = new Dictionary<Filey, string>();

            tt.Add(Filey.Filter, "ttttttt");

            var yy = tt.ToDictionary(ch => ch.Key.ToString(), ch => ch.Value);

            Console.WriteLine(Filey.Filter.ToString());
            /////ToDictionary(
            //p => p.Name,
            //           p => p.GetValue(queryParams)?.ToString() ?? "").


            //var content = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress.ToString() + urlBuilder.CollectionUrl())
            //{
            //    //  Content = new StringContent(JsonSerializer.Serialize(queryParams)),

            //};

            //var result = await _httpClient.SendAsync(content);

            //return await result.Content.ReadAsStringAsync();

            ////    var result = _httpClient.GetFromJsonAsync(
            //         _httpClient.BaseAddress.ToString() + urlBuilder.CollectionUrl()



            //        );
            var t = await _httpClient!.GetAsync(url);
            var top = await t.Content.ReadAsStringAsync();
            Console.WriteLine(top);
            return "";
            //var tt = new Query


        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="id">Records id which to be got</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> GetOne(string id)
        {
            var result = await GetFullList(id);
            return JsonSerializer.Deserialize<IDictionary<string, object>>(result, PbJsonOptions.options)!;
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
            return JsonSerializer.Deserialize<T>(result, PbJsonOptions.options)!;
        }

        /// <summary>
        /// Create new Record in the collection
        /// </summary>
        /// <param name="data">Data to be inserted</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> Create(IDictionary<string, object> data)
        {
            var response = await _httpClient.PostAsJsonAsync(urlBuilder.CollectionUrl(), data);
            return (await response.Content.ReadFromJsonAsync<IDictionary<string, object>>(PbJsonOptions.options))!;
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
            return (await response.Content!.ReadFromJsonAsync<T>(PbJsonOptions.options))!;
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
            return (await response.Content.ReadFromJsonAsync<T>(PbJsonOptions.options))!;
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

    }
}
