
using Microsoft.AspNetCore.Mvc;
using WebSocketApi.Services;

namespace WebSocketApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly WebSocketHandler _WsHandler;
        
        public NotificationController(WebSocketHandler wsHandler)
        {
            _WsHandler = wsHandler;
        }

        [HttpPost("notify")]
        public async Task<IActionResult> Notify([FromBody] object msg)
        {
            if(msg == null)
            {
                return NoContent();
            }

            var msgStr = msg.ToString();
            await _WsHandler.SendMessageAsync(msgStr);

            return Ok();
        }
    }
}
