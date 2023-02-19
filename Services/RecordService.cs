using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using pocketbase.net.Models.Helpers;

namespace pocketbase.net.Services
{
    internal class RecordService : BaseService
    {
        internal RealtimeService realtimeService { get; }
        public RecordService(
            HttpClient _httpClient,
            string collectionName,
            RealtimeService realtimeService
        ) : base(_httpClient, collectionName)
        {
            this.realtimeService = realtimeService;
        }


        //public Task<Record>()

    }
}