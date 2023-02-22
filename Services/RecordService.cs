using System;
using System.Net.Http;
using pocketbase.net.Models;

namespace pocketbase.net.Services
{
    public class RecordService : BaseService
    {
        internal RealtimeService realtimeService { get; }
        internal RecordService(
           HttpClient _httpClient,
           string collectionName,
           RealtimeService realtimeService
       ) : base(_httpClient, collectionName)
        {
            this.realtimeService = realtimeService;
        }



        /// <summary>
        /// subscribe to the realtime events of the server
        /// </summary>
        /// <param name="topic">topic representing colection</param>
        /// <param name="callbackFun">Action to be called when there is alterations in the given topic collection</param>
        public void Subscribe(string topic, Action<RealtimeEventArgs> callbackFun)
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