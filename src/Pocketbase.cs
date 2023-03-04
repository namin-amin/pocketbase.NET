using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services;

namespace pocketbase.net
{
    /// <summary>
    /// Represents the Cleint 
    /// </summary>
    public class Pocketbase
    {
        private HttpClient httpClient { get; }

        private readonly Dictionary<string, RecordService> RecordCollection = new();

        public BaseAuthService<AdminAuthModel> authStore { get; set; }

        private RealtimeService realtimeService { get; set; }

        public CollectionService collection { get; set; }

        public string baseurl { get; set; }
        public string lang { get; set; }
        /// <summary>
        /// Init Pocketbase Cleint
        /// </summary>
        /// <param name="baseurl">represents the baseurl of the Pocketbase server</param>
        /// <param name="lang">Language Preference</param>
        /// <param name="httpClient">Provide a HttpCleint to be used if already initialiased else pass null New one will be created</param>
        public Pocketbase(string baseurl,
                          string lang,
                          HttpClient? httpClient)
        {
            this.httpClient = httpClient ?? new HttpClient();
            this.baseurl = baseurl.EndsWith("/")? baseurl:baseurl+"/";
            this.httpClient.BaseAddress = new Uri(baseurl);
            this.lang = lang ?? "en-US";
            realtimeService = new(baseurl, this.httpClient);
            authStore = new(this.httpClient, "admins", this);
            collection = new(this.httpClient, this);
        }

        void FumSerivce(object? o, EventArgs e)
        {
            Console.WriteLine("test");
        }


        // public BaseAuthStore AuthStore { get; set; }

        /// <summary>
        /// Returns the Collection object that can be used to do CRUD and Subscriptions
        /// </summary>
        /// <param name="collectionname">Name of the collection to get</param>
        /// <returns></returns>
        public RecordService Collections(string collectionname)
        {
            RecordService? collectionService;
            if (RecordCollection.ContainsKey(collectionname))
            {
                collectionService = RecordCollection[collectionname];
                return collectionService;
            }

            collectionService = new(httpClient, collectionname, realtimeService, this);
            RecordCollection.Add(collectionname, collectionService);
            return collectionService;
        }



        public async Task<HttpResponseMessage> SendAsync(string url, HttpMethod httpMethod, StringContent? content = null)
        {
            try
            {
                var message = new HttpRequestMessage
                {
                    RequestUri = new Uri(baseurl!.ToString() + url),
                    Method = httpMethod,
                };

                if (content != null && httpMethod != HttpMethod.Get) message.Content = content;
                if (authStore.token != "")
                    message.Headers.Authorization = new AuthenticationHeaderValue(authStore.token);
                return await httpClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new(HttpStatusCode.InternalServerError);
            }

        }

    }

    public static class PocketBaseProvider
    {
        /// <summary>
        /// Extension method adds a Pocketbase To the Dependency injection services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseUrl">represents the baseurl of the Pocketbase server</param>
        /// <param name="httpClient">Provide a HttpCleint to be used if already initialiased else pass null New one will be created</param>
        /// <param name="lang">Language Preference</param>
        /// <returns></returns>
        public static IServiceCollection AddPocketbase(this IServiceCollection services, string baseUrl, HttpClient? httpClient, string lang = "en-US")
        {

            //return services.AddSingleton<Pocketbase>(new Pocketbase(baseUrl,lang));

            return services.AddSingleton((options) =>
            {
                return new Pocketbase(baseUrl, lang, httpClient);
            });
        }


        /// <summary>
        /// Extension method adds a Pocketbase To the Dependency injection services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="baseUrl">represents the baseurl of the Pocketbase server</param>
        /// <param name="lang">Language Preference</param>
        public static IServiceCollection AddPocketbase(this IServiceCollection services, string baseUrl, string lang = "en-US")
        {

            return services.AddSingleton((options) =>
            {
                return new Pocketbase(baseUrl, lang, new HttpClient());
            });
        }
    }
}
