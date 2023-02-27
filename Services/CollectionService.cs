using System.Net.Http;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services.Helpers;

namespace pocketbase.net.Services
{
    /// <summary>
    /// Collection service class defines the services available on the collection
    /// </summary>
    public class CollectionService
    {

        private readonly BaseService _baseService;

        public CollectionService(HttpClient httpClient, Pocketbase cleint)
        {
            _baseService = new BaseService(httpClient, "", cleint);
        }

        public async Task<ColRecord<CollectionModel>?> GetFullList(int batchSize = 100, RecordListQueryParams? queryParams = null)
        {
            var result = await _baseService.GetFullList(batchSize, queryParams);

            return Deserialize<ColRecord<CollectionModel>>(result, PbJsonOptions.options);
        }
        public async Task<ColRecord<CollectionModel>?> GetList(int page, int perPage, ListQueryParams? queryParams = null)
        {
            var result = await _baseService.GetList(page, perPage, queryParams);

            return Deserialize<ColRecord<CollectionModel>>(result, PbJsonOptions.options);
        }

        public async Task<ColRecord<CollectionModel>> GetFirstListItem(string filter, RecordListQueryParams? queryParams = null)
        {
            var result = await _baseService.GetFirstListItem(filter, queryParams);

            return Deserialize<ColRecord<CollectionModel>>(result, PbJsonOptions.options) ?? new();
        }
        public async Task<CollectionModel> GetOne(string collectionIdName)
        {
            var result = await _baseService.GetOne(collectionIdName);

            return Deserialize<CollectionModel>(Serialize(result)) ?? new();

        }
        public async Task<CollectionModel> Create(CollectionModel collection)
        {
            return await _baseService.Create<CollectionModel, CollectionModel>(collection);
        }

        public async Task<CollectionModel> Update(string collectionIdName, dynamic collectionChanges)
        {
            var result = await _baseService.Update<CollectionModel, dynamic>(collectionChanges, collectionIdName);
            return (CollectionModel)result;
        }

        public async Task<CollectionModel> Update<D>(string collectionIdName, D collectionChanges)
        {
            var result = await _baseService.Update<CollectionModel, D>(collectionChanges, collectionIdName);
            return (CollectionModel)result;
        }

        public async Task<bool> Delete(string collectionIdName)
        {
            return await _baseService.Delete(collectionIdName);
        }

        public async void Import(List<CollectionModel> collectionModels, bool deleteMissiong = false)
        {

            var result = Serialize<ImportModel>(new()
            {
                collectionModels = collectionModels,
                deleteMissiong = deleteMissiong
            }, PbJsonOptions.options);
            await _baseService._httpClient.PutAsync("/api/collections/import", new StringContent(result) { });
        }
        class ImportModel
        {
            public List<CollectionModel> collectionModels { get; set; } = new();
            public bool deleteMissiong { get; set; }
        }
    }

}