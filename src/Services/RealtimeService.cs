using System.Net.Http;
using pocketbase.net.Services.Interfaces;

namespace pocketbase.net.Services;

internal class RealtimeService : RealtimeServiceBase, IRealtimeServiceBase
{
    public RealtimeService(HttpClient httpClient, string baseUrl, Func<string>? authTokenProvider = null) : base(httpClient, baseUrl, authTokenProvider)
    {
    }
}
