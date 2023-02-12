using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using pocketbase.net.Models;

namespace pocketbase.net.Services
{
    public class RealtimeStreamer : IDisposable
    {


        private readonly string BaseUrl;

        private bool cancelled = false;

        /// <summary>
        /// private readonly 
        /// </summary>
        public RealtimeStreamer(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public void Dispose()
        {
            cancelled = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>

        /// </summary>
        public void Dowork(Action<SubscriptionEventArgs> callback)
        {
            //TODO for blazor implemet js interop for .net implement normal

            try
            {
                _ = Task.Run(async () =>
                          {
                              try
                              {
                                  await ReadSSE(callback);
                              }
                              catch (Exception ex)
                              {

                                  Console.WriteLine(ex.Message);
                              }
                              return Task.CompletedTask;
                          });
                ;

                //await Task.Yield();

                Console.WriteLine("runniong s");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }

        async Task ReadSSE(Action<SubscriptionEventArgs> callback)
        {

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "stream")
            {
                RequestUri = new Uri(BaseUrl + "realtime")
            };

            request.SetBrowserResponseStreamingEnabled(true);
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            bool prevNewLine = false;
            var text = "";

            Stream? stream = null;
            byte[] butes = new byte[1];
            while (!cancelled)
            {
                try
                {
                    stream = await response.Content.ReadAsStreamAsync();

                    if (stream.CanRead)
                    {
                        await stream.ReadAsync(butes);
                        string? letter = Encoding.UTF8.GetString(butes);
                        text += letter;

                        if (letter == Environment.NewLine && prevNewLine == true)
                        {
                            text = text[..^2];
                            Console.WriteLine(text);
                            var eventas = new SubscriptionEventArgs();
                            if (text.Contains("actions:created"))
                            {
                                eventas.Event = "creaed";

                            }
                            callback.Invoke(eventas);
                            text = string.Empty;
                            prevNewLine = false;
                            await Task.Delay(TimeSpan.FromMilliseconds(100));
                        }
                        else if (letter == Environment.NewLine && prevNewLine == false)
                        {
                            prevNewLine = true;
                        }
                        else
                        {
                            prevNewLine = false;
                        }
                    }
                    else
                    {
                        break;
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Retrying in 5 seconds");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }

            }

            if (stream is not null)
            {
                stream.Close();
                await stream.DisposeAsync();
            }

            client.CancelPendingRequests();
        }

    }

}
