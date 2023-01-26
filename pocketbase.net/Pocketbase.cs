using Microsoft.Extensions.DependencyInjection;
using pocketbase.net.Store;

namespace pocketbase.net
{
    public class Pocketbase
    {

        public Pocketbase(string baseurl, BaseAuthStore authStore, string lang = "en-US")
        {
            Baseurl = baseurl;
            AuthStore = authStore;
            Lang = lang;
        }
        public string Baseurl { get; set; }
        public string Lang { get; set; }
        public BaseAuthStore AuthStore { get; set; }

    }

    public static class PocketBaseProvider
    {
        public static IServiceCollection  AddPocketbase(this IServiceCollection services)
        {
         
            return services.AddSingleton<Pocketbase>();

        }
    }
}
