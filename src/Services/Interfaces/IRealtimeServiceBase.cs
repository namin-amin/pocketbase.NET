using pocketbase.net.Models;

namespace pocketbase.net.Services.Interfaces
{
    public interface IRealtimeServiceBase
    {
        void Dispose();
        void Subscribe(string topic, Action<RealtimeEventArgs> callback, string collectioName);
        void UnSubscribe(string topic);
    }
}