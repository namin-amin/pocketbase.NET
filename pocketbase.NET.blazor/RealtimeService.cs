using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using pocketbase.net.Services;
using pocketbase.net.Services.Interfaces;

namespace pocketbase.net.Blazor
{
    public class RealtimeService : RealtimeServiceBase,IRealtimeServiceBase
    {
        public RealtimeService(string baseUrl, HttpClient _httpcleint):base(_httpcleint ,baseUrl)
        {

        }
        public override void Dowork()
        {
            try
            {
                _ = Task.Run(async () =>
                          {
                              try
                              {
                                  var request = new HttpRequestMessage(HttpMethod.Get, "stream")
                                  {
                                      RequestUri = new Uri(baseUrl + "/api/realtime")
                                  };

                                  request.SetBrowserResponseStreamingEnabled(true);
                                  await ReadSSEStream(request);
                              }
                              catch (Exception ex)
                              {

                                  Console.WriteLine(ex.Message);
                                  Debug.WriteLine(ex.Message);
                              }
                              return Task.CompletedTask;
                          });
                ;

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                Console.WriteLine(ex.Message);
            }


        }
    }
}