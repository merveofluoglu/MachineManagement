using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketApi.Services
{
    public class WebSocketHandler
    {
        private readonly ConcurrentDictionary<Guid, WebSocket> _Sockets = new ConcurrentDictionary<Guid, WebSocket>();
    }
}
