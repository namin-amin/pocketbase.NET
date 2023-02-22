using System.Net.Http;
using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Services
{
    /// <summary>
    /// Collection service class defines the services available on the colllections
    /// </summary>
    public class CollectionService : BaseService
    {

        internal readonly RealtimeService realtimeService;
        internal CollectionService(HttpClient _httpClient,
            string collectionName,
            RealtimeService realtimeService) : base(_httpClient, collectionName)
        {
            this.realtimeService = realtimeService;
        }


    }

}