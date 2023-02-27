using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services.Helpers;
using static System.Text.Json.JsonSerializer;

namespace pocketbase.net.Services
{
    /// <summary>
    /// Base Service class defines all the basic operatins that a Service class shud be doing
    /// </summary>
    public class BaseService
    {
        internal Pocketbase cleint;
        internal readonly HttpClient _httpClient;
        public readonly string collectionName;

        private readonly UrlBuilder urlBuilder;


        public BaseService(HttpClient httpClient,
                           string collectionName, Pocketbase cleint)
        {
            this.collectionName = collectionName;
            _httpClient = httpClient;
            urlBuilder = new UrlBuilder(collectionName);
            this.cleint = cleint;
        }

        internal async Task<string> GetResponse(string id, IDictionary<string, string> queryParams)
        {

            var httpresult = await _httpClient!.GetAsync(urlBuilder.CollectionUrl(id, queryParams));
            return await httpresult.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="collectionName">name of the collection to be queried</param>
        /// <param name="id">If getting single record then required  id</param>
        /// <returns>Response schema based on the type of response</returns>
        public async Task<string> GetFullList(int BatchSize = 100, RecordListQueryParams? queryParams = null)
        {
            queryParams ??= new();
            var qParams = new
            Dictionary<string, string>()
            {
                {"page",queryParams.page.ToString()},
                {"perPage",BatchSize.ToString()},
                {"sort",queryParams.sort},
                {"filter",queryParams.filter},
                {"expand",queryParams.expand}
            };

            try
            {
                return await GetResponse("", qParams);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
                // throw;
            };

        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <typeparam name="T">collection objects Type</typeparam>
        /// <returns></returns>
        public async Task<Record<T>> GetFullList<T>(int BatchSize, RecordListQueryParams? queryParams = null) where T : PbBaseModel
        {
            var result = await GetFullList(BatchSize, queryParams);
            if (string.IsNullOrWhiteSpace(result))
            {
                return new();
            }
            return Deserialize<Record<T>>(result, PbJsonOptions.options) ?? new Record<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PerPage"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public async Task<string> GetList(int Page,
            int PerPage,
           ListQueryParams? queryParams = null)
        {
            queryParams ??= new();
            var qParams = new
             Dictionary<string, string>()
            {
                {"page",Page.ToString()},
                {"perPage",PerPage.ToString()},
                {"sort",queryParams.sort},
                {"filter",queryParams.filter}
            };

            string? list;
            try
            {
                var t = await _httpClient!.GetAsync(urlBuilder.CollectionUrl(queryParams: qParams));
                list = await t.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PerPage"></param>
        /// <param name="queryParams"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<Record<T>?> GetList<T>(int Page,
           int PerPage,
        ListQueryParams? queryParams = null) where T : PbBaseModel
        {

            try
            {
                return Deserialize<Record<T>>(await GetList(Page, PerPage, queryParams)) ?? null;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<Record<T>?> GetFirstListItem<T>(string filter, RecordListQueryParams? queryParams = null)
        where T : PbBaseModel
        {
            var result = await GetFirstListItem(filter, queryParams);
            return Deserialize<Record<T>>(result, PbJsonOptions.options);
        }

        public async Task<string> GetFirstListItem(string filter, RecordListQueryParams? queryParams = null)
        {
            queryParams ??= new();
            queryParams.filter = filter;
            var qParams = new
             Dictionary<string, string>()
            {
                {"page",queryParams.page.ToString()},
                {"perPage",queryParams.perPage.ToString()},
                {"sort",queryParams.sort},
                {"filter",queryParams.filter},
                {"expand",queryParams.expand}
            };

            return await GetResponse("", qParams);
        }



        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="id">Records id which to be got</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> GetOne(string id, string expand = "")
        {
            var result = await GetResponse(id, new Dictionary<string, string>()
            {
                {"expand",expand.ToString()}
            });
            return Deserialize<IDictionary<string, object>>(result, PbJsonOptions.options)!;
        }

        /// <summary>
        /// Gets Record response from PoccketBase
        /// </summary>
        /// <param name="id">Records id which to be got</param>
        /// <typeparam name="T">collection objects Type</typeparam>
        /// <returns></returns>
        public async Task<T> GetOne<T>(string id, string expand = "")
        {
            var result = await GetResponse(id, new Dictionary<string, string>()
            {
                {"expand",expand.ToString()}
            });
            return Deserialize<T>(result, PbJsonOptions.options)!;
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

                new StringContent(Serialize(data), System.Text.Encoding.UTF8, "application/json")
            );
            return (await response.Content.ReadFromJsonAsync<T>(PbJsonOptions.options))!;
        }
        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="id">id of the record to be deleted</param>
        /// <returns></returns>
        public async Task<bool> Delete(string id)
        {
            var result = await _httpClient.DeleteAsync(urlBuilder.CollectionUrl(id));
            return result.StatusCode == HttpStatusCode.OK;
        }

    }
}
