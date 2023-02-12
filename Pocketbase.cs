using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using pocketbase.net.Helpers;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services;
using pocketbase.net.Store;

namespace pocketbase.net
{
    public class Pocketbase
    {
        public HttpClient HttpClient { get; }

        private readonly Dictionary<string, CollectionService> CollectionsList = new();

        public Pocketbase(string baseurl,
                          string lang,
                          HttpClient? httpClient)
        {
            this.HttpClient = httpClient ?? new HttpClient();
            Baseurl = baseurl;
            this.HttpClient.BaseAddress = new Uri(baseurl);
            Lang = lang ?? "en-US";
        }



        public string Baseurl { get; set; }
        public string Lang { get; set; }
        // public BaseAuthStore AuthStore { get; set; }


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
        public static IServiceCollection AddPocketbase(this IServiceCollection services, string baseUrl, HttpClient? httpClient, string lang = "en-US")
        {

            //return services.AddSingleton<Pocketbase>(new Pocketbase(baseUrl,lang));

            return services.AddSingleton((options) =>
            {
                return new Pocketbase(baseUrl, lang, httpClient);
            });
        }

        public static IServiceCollection AddPocketbase(this IServiceCollection services, string baseUrl, string lang = "en-US")
        {

            return services.AddSingleton((options) =>
            {
                return new Pocketbase(baseUrl, lang, new HttpClient());
            });
        }
    }
}
