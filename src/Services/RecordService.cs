using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using pocketbase.net.Models;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services.Interfaces;

namespace pocketbase.net.Services
{
    public class RecordService : BaseService
    {
        internal IRealtimeServiceBase realtimeService { get; }
        internal BaseAuthService<RecordAuthModel> baseAuthService { get; }
        internal RecordService(
           HttpClient _httpClient,
           string collectionName,
           IRealtimeServiceBase realtimeService,
           Pocketbase cleint
       ) : base(_httpClient, collectionName, cleint)
        {
            this.realtimeService = realtimeService;
            baseAuthService = new(_httpClient, collectionName, cleint);
        }



        /// <summary>
        /// subscribe to the realtime events of the server
        /// </summary>
        /// <param name="topic">topic representing colection</param>
        /// <param name="callbackFun">Action to be called when there is alterations in the given topic collection</param>
        public void Subscribe(string topic, Action<RealtimeEventArgs> callbackFun)
        {
            realtimeService.Subscribe(topic, callbackFun, collectionName);
        }

        /// <summary>
        /// Unsubscribe from the current collection
        /// </summary>
        public void UnSubscribe(string topic)
        {
            realtimeService.UnSubscribe(topic);
        }

        public async Task<string> ListAuthMethods()
        {
            try
            {
                return await _httpClient.GetStringAsync($"api/collections/{collectionName}/auth-methods");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return "";
            }
        }

        public async Task<RecordAuthModel?> AuthWithPassword(
           string email,
           string password
       )
        {
            try
            {
                return await baseAuthService.AuthWithPassword(email, password, collectionName);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// Refresh currently authenticated users Auth details
        /// </summary>
        /// <returns></returns>
        public async Task<RecordAuthModel> AuthRefresh()
        {
            return await baseAuthService.AuthRefresh();
        }

        public async Task<string> AuthWithOAuth2()
        {

        }
    }
}