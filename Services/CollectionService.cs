using System.Collections.Generic;
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

        internal readonly RealtimeService realtimeService;
        internal CollectionService(HttpClient _httpClient,
            string collectionName,
            RealtimeService realtimeService) : base(_httpClient, collectionName)
        {
            this.realtimeService = realtimeService;
        }

        /// <summary>
        /// subscribe to the realtime events of the server
        /// </summary>
        /// <param name="topic">topic representing colection</param>
        /// <param name="callbackFun">Action to be called when there is alterations in the given topic collection</param>
        public void Subscribe(string topic, System.Action<RealtimeEventArgs> callbackFun)
        {
            realtimeService.Subscribe(topic, callbackFun);
        }

        /// <summary>
        /// Unsubscribe from the current collection
        /// </summary>
        public void UnSubscribe(string topic)
        {
            realtimeService.UnSubscribe(topic);
        }

    }

}