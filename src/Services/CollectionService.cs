using System.Net.Http;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services.Helpers;

namespace pocketbase.net.Services;

/// <summary>
/// Collection service class defines the services available on the collection
/// </summary>
public class CollectionService
{

    public readonly BaseService _baseService;

    public CollectionService(HttpClient httpClient, Pocketbase cleint)
    {
        _baseService = new BaseService(httpClient, "", cleint);
    }

    /// <summary>
    /// Get the full list of all collections 
    /// Note:Require admin permission
    /// </summary>
    /// <param name="batchSize">number of records to return</param>
    /// <param name="queryParams">extra args</param>
    /// <returns></returns>
    public async Task<ColRecord<CollectionModel>?> GetFullList(int batchSize = 100, RecordListQueryParams? queryParams = null)
    {
        var result = await _baseService.GetFullList(batchSize, queryParams);

        return Deserialize<ColRecord<CollectionModel>>(result, PbJsonOptions.options);
    }

    /// <summary>
    /// Returns the list of collection in a paginated way
    /// </summary>
    /// <param name="page">number of pages</param>
    /// <param name="perPage">number of recors to be fetched per page</param>
    /// <param name="queryParams">extra args</param>
    /// <returns></returns>
    public async Task<ColRecord<CollectionModel>?> GetList(int page, int perPage, ListQueryParams? queryParams = null)
    {
        var result = await _baseService.GetList(page, perPage, queryParams);

        return Deserialize<ColRecord<CollectionModel>>(result, PbJsonOptions.options);
    }

    /// <summary>
    /// Returns the first collection record matching the provided filter
    /// </summary>
    /// <param name="filter">filter to be matched</param>
    /// <param name="queryParams">ectra args</param>
    /// <returns></returns>
    public async Task<ColRecord<CollectionModel>> GetFirstListItem(string filter, RecordListQueryParams? queryParams = null)
    {
        var result = await _baseService.GetFirstListItem(filter, queryParams);

        return Deserialize<ColRecord<CollectionModel>>(result, PbJsonOptions.options) ?? new();
    }

    /// <summary>
    /// Get one collection record
    /// </summary>
    /// <param name="collectionIdName">collection id or collection name</param>
    /// <returns></returns>
    public async Task<CollectionModel> GetOne(string collectionIdName)
    {
        var result = await _baseService.GetOne(collectionIdName);

        return Deserialize<CollectionModel>(Serialize(result)) ?? new();

    }

    /// <summary>
    /// Crete new collection
    /// </summary>
    /// <param name="collection">new collection details</param>
    /// <returns></returns>
    public async Task<CollectionModel> Create(CollectionModel collection)
    {
        return await _baseService.Create<CollectionModel, CollectionModel>(collection);
    }

    /// <summary>
    /// update existing collection record
    /// </summary>
    /// <param name="collectionIdName">id / name of the collection to be updated</param>
    /// <param name="collectionChanges">new data to be updated to the collection</param>
    /// <returns></returns>
    public async Task<CollectionModel> Update(string collectionIdName, dynamic collectionChanges)
    {
        var result = await _baseService.Update<CollectionModel, dynamic>(collectionChanges, collectionIdName);
        return (CollectionModel)result;
    }

    /// <summary>
    /// update existing collection record
    /// </summary>
    /// <param name="collectionIdName">id/name of the collection to be updated</param>
    /// <param name="collectionChanges">new data to be updated to the collection</param>
    /// <typeparam name="D">DTO type of the collectionchanges object</typeparam>
    /// <returns></returns>
    public async Task<CollectionModel> Update<D>(string collectionIdName, D collectionChanges)
    {
        var result = await _baseService.Update<CollectionModel, D>(collectionChanges, collectionIdName);
        return (CollectionModel)result;
    }

    /// <summary>
    /// Delete and existing collection
    /// </summary>
    /// <param name="collectionIdName">collection id / name</param>
    /// <returns></returns>
    public async Task<bool> Delete(string collectionIdName)
    {
        return await _baseService.Delete(collectionIdName);
    }

    /// <summary>
    /// import all collection details
    /// </summary>
    /// <param name="collectionModels">list of collections </param>
    /// <param name="deleteMissiong"></param>
    /// <returns></returns>
    public async void Import(List<CollectionModel> collectionModels, bool deleteMissiong = false)
    {

        var result = Serialize<ImportModel>(new()
        {
            collectionModels = collectionModels,
            deleteMissiong = deleteMissiong
        }, PbJsonOptions.options);
        await _baseService.HttpClient.PutAsync("/api/collections/import", new StringContent(result) { });
    }
    class ImportModel
    {
        public List<CollectionModel> collectionModels { get; set; } = new();
        public bool deleteMissiong { get; set; }
    }
}