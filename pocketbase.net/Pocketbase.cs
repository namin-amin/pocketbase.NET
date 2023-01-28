using Microsoft.Extensions.DependencyInjection;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Store;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using pocketbase.net.Helpers;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace pocketbase.net
{
    public class Pocketbase
    {

        public Pocketbase(string baseurl, string lang = "en-US")
        {
            Baseurl = baseurl;
            //AuthStore = authStore;
            Lang = lang;
        }
        public string Baseurl { get; set; }
        public string Lang { get; set; }
        public BaseAuthStore AuthStore { get; set; }

        public async Task<ListModel<T>> GetFullList<T>() where T : PbBaseModel
        {
            var opt = new JsonSerializerOptions();
            opt.Converters.Add(new JsonDateTimeConverter());
            opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;


            var cleint =  new HttpClient();
            cleint.BaseAddress = new Uri(Baseurl);
            string? requesturl =  "collections/" + typeof(T).Name.ToLower() + "/records";
            var httpresult = await  cleint.GetAsync(requesturl);
            var result =  await httpresult.Content.ReadAsStringAsync();
            var tt =  JsonSerializer.Deserialize<ListModel<T>>(result,opt);
            return tt;
        }
    }

    public static class PocketBaseProvider
    {
        public static IServiceCollection  AddPocketbase(this IServiceCollection services , string baseUrl,string lang = "en-US" )
        {

            return services.AddSingleton<Pocketbase>(new Pocketbase(baseUrl,lang));

        }
    }
}
