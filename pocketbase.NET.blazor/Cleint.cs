using Microsoft.Extensions.DependencyInjection;
using pocketbase.net;
using pocketbase.net.Blazor;

namespace pocketbase.NET.blazor;

public static class PocketBaseProvider
{
    /// <summary>
    /// Extension method adds a Pocketbase To the Dependency injection services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="baseUrl">represents the baseurl of the Pocketbase server</param>
    /// <param name="httpClient">Provide a HttpClient to be used if already initialized else pass null New one will be created</param>
    /// <param name="lang">Language Preference</param>
    /// <returns></returns>
    public static IServiceCollection AddPocketbase(this IServiceCollection services, string baseUrl,
        HttpClient? httpClient = null, string lang = "en-US")
    {
        //return services.AddSingleton<Pocketbase>(new Pocketbase(baseUrl,lang));

        httpClient ??= new HttpClient();
        return services.AddSingleton((options)
            => new Pocketbase(baseUrl, lang, httpClient, new RealtimeService(baseUrl, httpClient)));
    }
}