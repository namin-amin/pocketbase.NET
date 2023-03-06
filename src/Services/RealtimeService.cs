using pocketbase.net.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase.net.Services
{
    internal class RealtimeService : RealtimeServiceBase, IRealtimeServiceBase
    {
        public RealtimeService(HttpClient httpClient, string baseUrl) : base(httpClient, baseUrl)
        {
        }
    }
}
