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
}
