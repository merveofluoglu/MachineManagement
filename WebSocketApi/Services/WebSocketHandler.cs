using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketApi.Services
{

    public class WebSocketHandler
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public WebSocketHandler(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }


    //public class WebSocketHandler
    //{
    //    private readonly ConcurrentDictionary<Guid, WebSocket> _Sockets = new ConcurrentDictionary<Guid, WebSocket>();

    //    public async Task HandleAsync(WebSocket ws)
    //    {
    //        var socketId = Guid.NewGuid();
    //        _Sockets.TryAdd(socketId, ws);

    //        await ReceiveAsync(ws, async (res, buff) =>
    //        {
    //            if(res.MessageType == WebSocketMessageType.Text)
    //            {
    //                var msg = Encoding.UTF8.GetString(buff, 0, res.Count);
    //                // Rest of the code
    //            }
    //            else if(res.MessageType == WebSocketMessageType.Close)
    //            {
    //                _Sockets.TryRemove(socketId, out _);
    //                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket closed by the server.", CancellationToken.None);
    //            }
    //        });

    //    }

    //    private async Task ReceiveAsync(WebSocket ws, Action<WebSocketReceiveResult, byte[]> handleMessage)
    //    {
    //        var buff = new byte[1024*4];
    //        while (ws.State != WebSocketState.Open)
    //        {
    //            var res = await ws.ReceiveAsync(new ArraySegment<byte>(buff), CancellationToken.None);
    //            handleMessage(res, buff);
    //        }
    //    }

    //    public async Task SendMessageAsync(string msg)
    //    {
    //        var bytes = Encoding.UTF8.GetBytes(msg);
    //        foreach(var socket in _Sockets.Values)
    //        {
    //            if(socket.State == WebSocketState.Open)
    //            {
    //                await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    //            }
    //        }
    //    }
    //}
}
