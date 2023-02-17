using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using pocketbase.net.Services;
using pocketbase.net.Store;

namespace pocketbase.net
{
    /// <summary>
    /// Represents the Cleint 
    /// </summary>
    public class Pocketbase
    {
        private HttpClient HttpClient { get; }

        private readonly Dictionary<string, CollectionService> CollectionsList = new();

        public BaseAuthService AuthStore { get; set; }

        public string Baseurl { get; set; }
        public string Lang { get; set; }
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
            HttpClient = httpClient ?? new HttpClient();
            Baseurl = baseurl;
            HttpClient.BaseAddress = new Uri(baseurl);
            Lang = lang ?? "en-US";
            AuthStore = new(HttpClient, "admins");
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
        public CollectionService Collections(string collectionname)
        {
            CollectionService? collectionService;
            if (CollectionsList.ContainsKey(collectionname))
            {
                collectionService = CollectionsList[collectionname];
                return collectionService;
            }

            collectionService = new CollectionService(HttpClient, collectionname);
            CollectionsList.Add(collectionname, collectionService);
            return collectionService;
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
