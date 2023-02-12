using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using pocketbase.net.Models;

namespace pocketbase.net.Services
{
    public class RealtimeService
    {

        private readonly Dictionary<string, List<Action<SubscriptionEventArgs>>> subscriptions = new();
        public string BaseUrl { get; }
        private RealtimeStreamer Sse { get; }

        public RealtimeService(string baseUrl)
        {
            BaseUrl = baseUrl;
            Sse = new RealtimeStreamer(BaseUrl);

        }

        public void Subscribe(string topic, Action<SubscriptionEventArgs> callback)
        {



            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new Exception("topic needs to be set cannot be empty");
            }

            topic = topic.ToLower();
            try
            {
                if (subscriptions.Count == 0)
                {
                    //Todo here for each topic need call related callbacks
                    subscriptions.Add(topic, new List<Action<SubscriptionEventArgs>>());
                    subscriptions[topic].Add(callback);
                    Sse.Dowork(callback);


                }
                else if (subscriptions.TryGetValue(
                    topic,
                    out List<Action<SubscriptionEventArgs>> value))
                {
                    value?.Add(callback);
                }
                else
                {
                    subscriptions.Add(topic, new List<Action<SubscriptionEventArgs>>());
                    subscriptions[topic].Add(callback);
                }




            }
            catch (System.Exception)
            {

                Console.WriteLine("Error thrown");
            }


            void AddOrRemoveTopics()
            {

            }

        }



        public void UnSubscribeAll()
        {
            // foreach (var sse in streams)
            // {
            //     sse.Dispose();
            // }

            Console.WriteLine("All discposed");
        }
    }
}